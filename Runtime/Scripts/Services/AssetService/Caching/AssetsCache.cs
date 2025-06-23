using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Constants;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Exceptions;
using Bridge.Services.Advertising;
using Bridge.Services.AssetService.Caching.AssetReaders;
using Bridge.Services.AssetService.Caching.CachePathGeneration;
using Bridge.Services.AssetService.Caching.Encryption;
using ProtoBuf;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.Services.AssetService.Caching
{
    internal interface ISongsAdsCache
    {
        bool HasCached(SongAdData songData);
        Task<Texture2D> GetBanner(SongAdData songData);
        Task Cache(SongAdData songData, Texture2D texture);
    }

    internal sealed class AssetsCache : ISongsAdsCache
    {
        private const string CACHE_DATA_FILE_NAME = "Cache.data";

        private readonly string _rootFolder;
        private readonly AssetReaderProvider _assetReaderProvider;
        private readonly LocalFilesPathProvider _localFilesPathGeneration;
        private readonly ISerializer _serializer;
        private readonly BannersCache _bannersCache;
        private readonly EncryptionService _encryptionService;
        private readonly IFileWriter _fileWriter;

        private CachedFilesData _cachedFilesData;
        private bool _cacheDataFileLocked;
        private bool _updateVersionFileAgain;

        public readonly FFEnvironment Environment;
        public event Action Updated;
        public IEnumerable<FileData> FilesData => _cachedFilesData.FileDatas;

        public AssetsCache(string rootFolder, FFEnvironment environment, AssetReaderProvider assetReaderProvider,
            ISerializer serializer, EncryptionService encryptionService)
        {
            _rootFolder = rootFolder;
            Environment = environment;
            _assetReaderProvider = assetReaderProvider;
            _localFilesPathGeneration = new LocalFilesPathProvider(environment);
            _serializer = serializer;
            _bannersCache = new BannersCache(environment, serializer);
            _encryptionService = encryptionService;
            _fileWriter = new FileWriter();
            LoadCacheData();
        }

        public bool HasInCache<T>(T model, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            var fileData = GetFileData(model, fileInfo);
            return fileData != null;
        }

        public FileData GetFileData<T>(T model, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            var relativePath = GetRelativeFilePath(model.GetType(), model.Id, fileInfo);
            var fileData = _cachedFilesData.GetFileData(relativePath);
            if (fileData == null) return null;
            return fileData.Version == fileInfo.Version ? fileData : null;
        }

        public Task SaveToCacheAsync<T>(T model, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            return SaveToCacheAsync(model.Id, model.GetType(), fileInfo);
        }

        public async Task SaveToCacheAsync(long entityId, Type entityType, FileInfo fileInfo)
        {
            var fullPath = PrepareFilePath(entityId, entityType, fileInfo);
            var writer = GetFileWriter(fullPath, entityType);

            if (fileInfo.FileRawData != null)
            {
                await writer.WriteFileBytesAsync(fullPath, fileInfo.FileRawData);
                fileInfo.FileRawData = null; //clear from ram
            }
            else if (fileInfo.FilePath != null && fileInfo.Exists())
            {
                await writer.CopyLocalFileAsync(fullPath, fileInfo.FilePath);
            }
            else if (fileInfo.Source?.CopyFrom != null)
            {
                var copyFrom = fileInfo.Source.CopyFrom;
                var type = entityType;
                var id = copyFrom.Id;
                var sourcePath = GetFilePath(type, id, fileInfo);
                //save to cache only if source file is cached as well, but do not download now
                if (!File.Exists(sourcePath)) return;
                await writer.CopyLocalFileAsync(fullPath, sourcePath);
            }
            else
            {
                Debug.LogError("File info does not have local file or raw data byte[]");
                return;
            }

            if (string.IsNullOrEmpty(fileInfo.Version))
            {
                Debug.LogError($"Asset {entityType.Name} should not have version id NULL/Empty");
                return;
            }

            OnFileUpdated(entityId, entityType, fileInfo);
        }

        public async Task SaveToCacheAsync<T>(T model, FileInfo fileInfo, Stream stream, CancellationToken cancellationToken) where T : IFilesAttachedEntity
        {
            var fullPath = PrepareFilePath(model, fileInfo);
            var writer = GetFileWriter(fullPath, model.GetType());

            await writer.WriteFileFromStreamAsync(fullPath, stream, cancellationToken);

            if (string.IsNullOrEmpty(fileInfo.Version))
            {
                Debug.LogError($"Asset {model.GetType().Name} should not have version id NULL/Empty");
                return;
            }

            OnFileUpdated(model, fileInfo);
        }

        private void OnFileUpdated<T>(T model, FileInfo fileInfo)
            where T : IFilesAttachedEntity
        {
            var assetType = model.GetType();
            OnFileUpdated(model.Id, assetType, fileInfo);
        }

        private void OnFileUpdated(long entityId, Type entityType, FileInfo fileInfo)
        {
            var relativePath = GetRelativeFilePath(entityType, entityId, fileInfo);
            var fullPath = GetFullPath(relativePath); 
            
            GetFileInfo(fullPath, out var sizeKb, out var creatingDate, out var lastUsedDate);
            
            _cachedFilesData.TrackFileData(relativePath, fileInfo.Version, entityType, entityId, sizeKb, creatingDate, lastUsedDate);
            Updated?.Invoke();
        }

        private void GetFileInfo(string filePath, out long sizeKb, out DateTime creatingDate, out DateTime lastUsedDate)
        {
            var fileInfo = new System.IO.FileInfo(filePath);
            sizeKb = fileInfo.Length / 1024;
            creatingDate = fileInfo.CreationTimeUtc;
            lastUsedDate = fileInfo.LastAccessTimeUtc;
        }
        
        private string PrepareFilePath<T>(T model, FileInfo fileInfo)where T: IFilesAttachedEntity
        {
            return PrepareFilePath(model.Id, model.GetType(), fileInfo);
        }

        private string PrepareFilePath(long entityId, Type entityType, FileInfo fileInfo)
        {
            var fullPath = GetFilePath(entityType, entityId, fileInfo);
            CreateDirectoryIfNotExists(fullPath);
            return fullPath;
        }

        public async Task<GetAssetResult> GetFile<T>(T model, FileInfo fileInfo, StreamingType? streamingType = null, CancellationToken cancellationToken = default) where T: IFilesAttachedEntity
        {
            if (!HasInCache(model, fileInfo))
            {
                throw new Exception($"Trying to GET to cache object{model.GetType().Name} with Id {model.Id}, which is not cached ");
            }

            var filePath = GetFilePath(model.GetType(), model.Id, fileInfo);
            var extension = _encryptionService.TryGetEncryptedExtension(filePath, out var encryptedExtension)
                ? encryptedExtension
                : fileInfo.Extension.ToExtensionString();
            var reader = _assetReaderProvider.GetReader(extension);
            if (reader == null)
            {
                throw new Exception("Can't read file with extension: " + extension);
            }
            
            try
            {
                await reader.Read(filePath, cancellationToken);
            }
            catch (Exception e)
            {
                if (e is FileEncryptionException)
                {
                    // redirecting to log handler for further processing on client side
                    Debug.LogError($"{FileEncryptionException.BuildErrorMessage(e.Message)}");
                }
                return e is OperationCanceledException ? new CanceledGetAssetResult() : new GetAssetResult(e.Message);
            }
           
            _cachedFilesData.IncrementUsedCount(GetRelativeFilePath(model.GetType(), model.Id, fileInfo));

            return reader.ProvidesUnityObject 
                ? new GetAssetResult(reader.Asset, filePath) 
                : new GetAssetResult(reader.RawData, filePath, streamingType ,reader.RawData);
       }

        public bool HasCached(SongAdData songData)
        {
            return _bannersCache.HasCached(songData);
        }

        public Task<Texture2D> GetBanner(SongAdData songData)
        {
            return _bannersCache.Get(songData);
        }

        public Task Cache(SongAdData songData, Texture2D texture)
        {
            return _bannersCache.Cache(songData, texture);
        }
        
        public Result Clear(FFEnvironment? environment = null)
        {
            var targetEnvironments = environment.HasValue
                ? new[] {environment.Value}
                : Enum.GetValues(typeof(FFEnvironment)).Cast<FFEnvironment>().ToArray();
            
            try
            {
                if (_cachedFilesData != null)
                {
                    _cachedFilesData.Reset();
                    SaveAssetsMetaData();
                }
                
                foreach (var env in targetEnvironments)
                {
                    var path = GetEnvironmentCachePath(env);
                    if (!Directory.Exists(path)) continue;
                    DropAllAssetFiles(env);
                    var dataFilePath = GetCacheDataFilePath(env);
                    if (File.Exists(dataFilePath))
                    {
                        File.Delete(dataFilePath);
                    }
                }
                
                _bannersCache.Clear();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message); 
            }
            return new SuccessResult();
        }

        public void ClearAssetBundles(FFEnvironment env)
        {
            var path = GetEnvironmentCachePath(env);
            if (!Directory.Exists(path)) return;

            var assetBundleFiles = FilesData.Where(x => x.Path.Contains(FileNameConstants.ASSET_BUNDLE_FILE_NAME));
            Delete(assetBundleFiles.ToArray());
        }

        private void DropAllAssetFiles(FFEnvironment env)
        {
            var envRootFolder = GetEnvironmentCachePath(env);
            var rootFolder = new DirectoryInfo(envRootFolder);
            var subDirectories = rootFolder.GetDirectories();
            var subFolderNames = subDirectories.Select(dir => dir.Name).ToArray();
            foreach (var subFolderName in subFolderNames)
            {
                if (!_localFilesPathGeneration.IsAssetStorageFolder(subFolderName)) continue;

                var assetMainDirectory = new DirectoryInfo(Path.Combine(envRootFolder, subFolderName));
                assetMainDirectory.Delete(true);
            }
        }

        private void LoadCacheData()
        {
            LoadAssetsData();
            LoadBanners();
        }

        private void LoadBanners()
        {
            _bannersCache.LoadCache();
        }

        private void LoadAssetsData()
        {
            var path = GetCacheDataFilePath(Environment);
            if (!File.Exists(path))
            {
                _cachedFilesData = new CachedFilesData();
                return;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    var dataList = _serializer.DeserializeProtobuf<List<FileData>>(stream);
                    _cachedFilesData = new CachedFilesData(dataList);
                }
            }
            catch (Exception)
            {
                _cachedFilesData = new CachedFilesData();
            }
        }

        public string GetFilePath(Type assetType, long id, FileInfo fileInfo)
        {
            var localPath = GetRelativeFilePath(assetType, id, fileInfo);
            return GetFullPath(localPath);
        }

        public string GetFullPath(FileData fileData)
        {
            return GetFullPath(fileData.Path);
        }

        private string GetFullPath(string relativePath)
        {
            return Path.Combine(UnityConstants.PersistentDataPath, _rootFolder, relativePath).Replace('\\','/');
        }
        
        private string GetRelativeFilePath(Type assetType, long id, FileInfo fileInfo)
        { 
            var relativePath = _localFilesPathGeneration.GetPath(assetType, id, fileInfo);
            return fileInfo.FileType == FileType.MainFile && _encryptionService.ShouldEncrypt(assetType)
                ? $"{relativePath}{_encryptionService.TargetExtension}"
                : relativePath;
        }

        private void CreateDirectoryIfNotExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void SaveCacheDataToFile()
        {
            SaveAssetsMetaData();
            _bannersCache.SaveMetadata();
        }

        private void SaveAssetsMetaData()
        {
            if (_cacheDataFileLocked)
            {
                _updateVersionFileAgain = true;
                return;
            }

            _cacheDataFileLocked = true;

            var path = GetCacheDataFilePath(Environment);
            CreateDirectoryIfNotExists(path);
            var copiedData = _cachedFilesData.FileDatas.Select(x => x).ToArray();
            var bytes = _serializer.SerializeProtobuf(copiedData);
            File.WriteAllBytes(path, bytes);

            if (_updateVersionFileAgain)
            {
                SaveCacheDataToFile();
            }
            else
            {
                _cacheDataFileLocked = false;
            }
        }

        private string GetCacheDataFilePath(FFEnvironment environment)
        {
            return Path.Combine(GetEnvironmentCachePath(environment), CACHE_DATA_FILE_NAME);
        }
        
        private string GetEnvironmentCachePath(FFEnvironment environment)
        {
            return Path.Combine(GetRootFolderPath(), environment.ToString());
        }
        
        private string GetRootFolderPath()
        {
            return Path.Combine(UnityConstants.PersistentDataPath, _rootFolder);
        }

        public Task<long> GetCacheSizeKbAsync()
        {
            return Task.Run(GetCacheSizeKb);
        }

        private long GetCacheSizeKb()
        {
            return _cachedFilesData.FileDatas.Sum(x => x.SizeKb);
        }

        public Task Delete(FileData[] filesData)
        {
            return Task.Run(() =>
            {
                foreach (var fileData in filesData)
                {
                    var filePath = GetFullPath(fileData.Path);
                    _cachedFilesData.ForgetFileData(fileData);
                    if (!File.Exists(filePath)) return;
                    File.Delete(filePath);
                }
            });
        }

        public async Task Delete(FileData fileData)
        {
            var filePath = GetFullPath(fileData.Path);
            _cachedFilesData.ForgetFileData(fileData);
            await Task.Run(() =>
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            });
        }

        public string GetCachedFileFullPath(FileData fileData)
        {
            return GetFullPath(fileData.Path);
        }

        private IFileWriter GetFileWriter(string fullPath, Type assetType)
        {
            var encrypted = fullPath.EndsWith(_encryptionService.TargetExtension);
            var partialEncrypted = _encryptionService.IsPartiallyEncrypted(assetType);

            return encrypted
                ? partialEncrypted ? _encryptionService.GetPartialEncryptedFileWriter() : _encryptionService.GetEncryptedFileWriter()
                : _fileWriter;
        }
    }
}