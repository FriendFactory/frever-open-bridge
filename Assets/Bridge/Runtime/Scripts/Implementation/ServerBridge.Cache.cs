using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BestHTTP;
using BestHTTP.Caching;
using Bridge.Models.Common;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;
using Bridge.Services.AssetService.Caching.CachePathGeneration;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private const string ROOT_CACHE_FOLDER = "Cache";
        private bool IsCacheInitialized => _assetsCache!=null && _assetsCache.Environment == Environment;
        
        public bool IsCacheEmpty
        {
            get
            {
                if (!IsCacheInitialized)
                {
                    InitializeCacheForSelectedEnvironment();
                }
                return _assetsCache.FilesData == null || !_assetsCache.FilesData.Any();
            }
        }

        public async Task<Result> ClearCacheAsync(bool deleteUserLoginData = false,
            FFEnvironment? targetEnvironment = null, params CacheType[] cachesToClear)
        {
            if (deleteUserLoginData)
                _userDataStorage.Clear();

            if (cachesToClear == null || cachesToClear.Length == 0)
            {
                cachesToClear = Enum.GetValues(typeof(CacheType)).Cast<CacheType>().ToArray();
            }

            try
            {
                foreach (var cacheType in cachesToClear)
                {
                    await ClearCache(targetEnvironment, cacheType);
                }
                
                HTTPCacheService.BeginClear();
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }

            return new SuccessResult();
        }

        public async Task<Result> ClearAssetBundleAsync()
        {
            try
            {
                await Task.Run(() => _assetsCache.ClearAssetBundles(Environment));
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            
            return new SuccessResult();
        }

        public async Task<long> GetCacheSizeKb()
        {
            var assetsCacheSizeKb = await _assetsCache.GetCacheSizeKbAsync();
            var httpCacheSizeKb = HTTPCacheService.GetCacheSize() / 1024;
            var storageSizeKb = _storageFileCache.GetSizeKb();
            return assetsCacheSizeKb + (long)httpCacheSizeKb + storageSizeKb;
        }

        public Task<FileData> GetCachedFileDataAsync<T>(T target, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            return Task.Run(() => GetCachedFileData(target, fileInfo));
        }

        public FileData GetCachedFileData<T>(T target, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            return _assetsCache.GetFileData(target, fileInfo);
        }

        public IEnumerable<FileData> GetCachedFilesData()
        {
            return _assetsCache.FilesData;
        }

        public IEnumerable<FileData> GetCachedFilesData<T>() where T : IFilesAttachedEntity
        {
            var assetTypeName = GetAssetTypeName(typeof(T));
            return _assetsCache.FilesData.Where(x => x.AssetTypeName == assetTypeName);
        }

        public string GetFilePath<T>(T target, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            if (TryGetPathForAsset(target, fileInfo, out var path))
            {
                return path;
            }

            if (TryGetPathForAssetOrigin(target, fileInfo, out path))
            {
                return path;
            }
            
            throw new FileNotFoundException($"Cache couldn't find file for {target.GetType().Name} {target.Id} {fileInfo.FileType}");
        }

        public string GetCachedFileFullPath(FileData fileData)
        {
            return _assetsCache.GetCachedFileFullPath(fileData);
        }

        public string GetAssetTypeUnifiedName(IFilesAttachedEntity entity)
        {
            return GetAssetTypeUnifiedName(entity.GetType());
        }

        public string GetAssetTypeUnifiedName(Type entityType)
        {
            return Config.Settings.TryGetValue(entityType, out var data) ? data.UnifiedAssetTypeName : null;
        }

        public Task DeleteAllFromCache<T>() where T : IFilesAttachedEntity
        {
            var files = GetCachedFilesData<T>();
            return DeleteFromCache(files.ToArray());
        }

        public Task DeleteFromCache(FileData[] filesData)
        {
            return _assetsCache.Delete(filesData);
        }

        public Task DeleteFromCache(FileData fileData)
        {
            return _assetsCache.Delete(fileData);
        }

        public void DeleteTempFiles()
        {
            _tempFileCache.Clear();
        }

        partial void InitializeCacheForSelectedEnvironment()
        {
            if (IsCacheInitialized) return;
            _assetsCache = new AssetsCache(ROOT_CACHE_FOLDER, Environment, _assetReaderProvider, _serializer, _encryptionService);
        }
        
        private bool TryGetPathForAsset<T>(T target, FileInfo fileInfo, out string path) where T : IFilesAttachedEntity
        {
            var cachedFileData = GetCachedFileData(target, fileInfo);
            path = cachedFileData != null ? GetCachedFileFullPath(cachedFileData) : null;
            return !string.IsNullOrEmpty(path);
        }

        private bool TryGetPathForAssetOrigin(IFilesAttachedEntity target, FileInfo fileInfo, out string path)
        {
            var source = fileInfo.Source.CopyFrom;

            var assetTypeName = GetAssetTypeName(target.GetType());
            var allCached = GetCachedFilesData();
            var fileData = allCached.FirstOrDefault(x =>
                x.AssetId == source.Id && x.AssetTypeName == assetTypeName && x.Version == source.Version);
            path = fileData != null ? GetCachedFileFullPath(fileData) : null;
            return !string.IsNullOrEmpty(path);
        }

        private static string GetAssetTypeName(Type entityType)
        {
            return Config.Settings[entityType].UnifiedAssetTypeName;
        }
        
        private async Task ClearCache(FFEnvironment? targetEnvironment, CacheType cacheType)
        {
            switch (cacheType)
            {
                case CacheType.AssetFiles:
                    var resp = await Task.Run(() => _assetsCache.Clear(targetEnvironment));
                    if (resp.IsError)
                    {
                        throw new InvalidOperationException($"Failed to clear cache. Reason: {resp.ErrorMessage}");
                    }
                    break;
                case CacheType.FilesStoredByKey:
                    _storageFileCache.ClearCache();
                    break;
                case CacheType.TranscodingFiles:
                    await _transcoder.CleanCacheFromConvertedFiles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}