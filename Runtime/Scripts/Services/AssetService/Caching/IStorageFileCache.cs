using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Constants;
using Bridge.Results;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching
{
    internal interface IStorageFileCache
    {
        long GetSizeKb();
        bool HasInCache(StorageFileInfo storageFile);
        bool HasInCache(string key);
        Task SaveToCacheAsync(StorageFileInfo storageFileInfo, byte[] bytes);
        Task<byte[]> GetRawDataAsync(StorageFileInfo storageFileInfo, CancellationToken token);
        Result<byte[]> GetImageBytesFromCache(string key);
        void DeleteAllVersions(StorageFileInfo storageFileInfo);
        void ClearCache();
    }
    
    internal sealed class StorageFileCache: IStorageFileCache
    {
        private const string STORAGE_FILES_FOLDER = "StorageFiles";
        
        private readonly string _rootCacheDirectory;
        private readonly FFEnvironment _environment;

        public StorageFileCache(string rootCacheDirectory, FFEnvironment environment)
        {
            _rootCacheDirectory = rootCacheDirectory;
            _environment = environment;
        }

        public long GetSizeKb()
        {
            var folderPath = GetMainFolder();
            if (!Directory.Exists(folderPath)) return 0;
            
            long totalSizeBytes = 0;
            var allFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            for (var i = 0; i < allFiles.Length; i++)
            {
                var fileInfo = new FileInfo(allFiles[i]);
                totalSizeBytes += fileInfo.Length;
            }

            return totalSizeBytes / 1024;
        }

        public bool HasInCache(StorageFileInfo storageFile)
        {
            var path = GetFullPath(storageFile);
            return File.Exists(path);
        }

        public bool HasInCache(string key)
        {
            var folderWithVersions = GetPathForFile(key);
            if (!Directory.Exists(folderWithVersions)) return false;
            var versions = GetExistedFileVersions(key);
            return versions.Any();
        }

        public async Task<byte[]> GetRawDataAsync(StorageFileInfo storageFile, CancellationToken token)
        {
            var path = GetFullPath(storageFile);
            using (var fs = File.OpenRead(path))
            {
                var buff = new byte[fs.Length];
                await fs.ReadAsync(buff, 0, (int) fs.Length, token);
                return buff;
            }
        }

        public Result<byte[]> GetImageBytesFromCache(string key)
        {
            var existedVersions = GetExistedFileVersions(key).ToArray();
            if (existedVersions.Length > 1)
            {
                Debug.LogWarning($"More than 1 file version is stored in the cache");
            }

            var bytes = File.ReadAllBytes(existedVersions.First());
            return Result<byte[]>.Success(bytes);
        }

        public Task SaveToCacheAsync(StorageFileInfo storageFile, byte[] bytes)
        {
            var path = GetFullPath(storageFile);
            CreateDirectoryIfNotExists(path);
            return WriteFileBytes(path, bytes);
        }

        public void DeleteAllVersions(StorageFileInfo fileInfo)
        {
            var fileMainFolder = GetPathForFile(fileInfo.Key);
            if (!Directory.Exists(fileMainFolder)) return;
            
            Directory.Delete(fileMainFolder, true);
        }

        public void ClearCache()
        {
            var mainFolder = GetMainFolder();
            if (!Directory.Exists(mainFolder)) return;
            
            var directory = new DirectoryInfo(mainFolder);
            directory.Delete(true);
        }

        private async Task WriteFileBytes(string filePath, byte[] bytes)
        {
            using (var file = File.Create(filePath))
            {
                await file.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        
        private void CreateDirectoryIfNotExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string GetFullPath(StorageFileInfo storageFileInfo)
        {
            return Path.Combine(GetPathForFile(storageFileInfo.Key), storageFileInfo.Version).Replace('\\','/');
        }

        private string GetPathForFile(string fileKey)
        {
            return Path.Combine(GetMainFolder(), fileKey).Replace('\\','/');
        }
        
        private string GetMainFolder()
        {
            return Path.Combine(UnityConstants.PersistentDataPath, _rootCacheDirectory, _environment.ToString(), STORAGE_FILES_FOLDER);
        }

        private IEnumerable<string> GetExistedFileVersions(string key)
        {
            var folderWithVersions = GetPathForFile(key);
            return Directory.EnumerateFileSystemEntries(folderWithVersions);
        }
    }
}