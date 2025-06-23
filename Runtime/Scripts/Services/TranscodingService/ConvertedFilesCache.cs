using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace Bridge.Services.TranscodingService
{
    internal class ConvertedFilesCache
    {
        private readonly Dictionary<byte[], ConvertedFileCacheEntry> _cachedFiles = new Dictionary<byte[], ConvertedFileCacheEntry>();
        private const string STORE_FILES_ROOT_FOLDER = "ConvertedFiles";
        private static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;//need store here to get it from non main thread

        private string StoredFilesFolderPath => Path.Combine(PERSISTENT_DATA_PATH, STORE_FILES_ROOT_FOLDER);

        public bool HasConvertedFileFor(string originFilePath, out ConvertedFileCacheEntry cacheEntry)
        {
            if (_cachedFiles.Count == 0)
            {
                cacheEntry = null;
                return false;
            }

            var fileHash = GetHashCode(originFilePath);

            var cachedFileData = _cachedFiles.FirstOrDefault(x => IsEqual(x.Key, fileHash));
            cacheEntry = cachedFileData.Value;
            return cacheEntry != null && File.Exists(cacheEntry.FilePath);
        }

        private bool IsEqual(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length) return false;
           
            var i=0;
            while ((i < hash1.Length) && (hash1[i] == hash2[i]))
            {
                i += 1;
            }
            return i == hash1.Length;
        }
        
        public async Task<string> SaveFileAsync(HttpContent httpContent, string fileExtension, string originFilePath, string uploadId)
        {
            var fileDestPath = Path.GetFullPath(StoredFilesFolderPath + $"/{Guid.NewGuid()}{fileExtension}");

            if (!Directory.Exists(StoredFilesFolderPath))
                Directory.CreateDirectory(StoredFilesFolderPath);
            
            using (var fs = new FileStream(fileDestPath, FileMode.CreateNew))
            {
                await httpContent.CopyToAsync(fs);
            }
            
            var hash = GetHashCode(originFilePath);
            _cachedFiles.Add(hash, new ConvertedFileCacheEntry(fileDestPath, uploadId));
            
            return fileDestPath;
        }

        private byte[] GetHashCode(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                using (var hashAlgorithm = SHA256.Create())
                {
                    return hashAlgorithm.ComputeHash(stream);
                }
            }
        }
        
        public void CleanCacheFromConvertedFilesInternal()
        {
            _cachedFiles.Clear();
            
            var dir = new DirectoryInfo(StoredFilesFolderPath);
            if (!dir.Exists)
                return;

            foreach (var file in dir.GetFiles())
            {
                file.Delete(); 
            }
            foreach (var d in dir.GetDirectories())
            {
                d.Delete(true); 
            }
        }
    }
}