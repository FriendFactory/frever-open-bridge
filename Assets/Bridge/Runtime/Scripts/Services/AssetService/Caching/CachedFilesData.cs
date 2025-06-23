using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Bridge.Services.AssetService.Caching.CachePathGeneration;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching
{
    [Serializable]
    internal sealed class CachedFilesData
    {
        public IEnumerable<FileData> FileDatas => _fileDatas.Values;
        private ConcurrentDictionary<string,FileData> _fileDatas;
        private readonly IReadOnlyDictionary<Type, AssetSetting> _cacheSettings;
        
        public CachedFilesData(IEnumerable<FileData> data = null)
        {
            _cacheSettings = Config.Settings;
            _fileDatas = new ConcurrentDictionary<string, FileData>();
            
            if (data == null) return;
            foreach (var fileData in data)
            {
                AddData(fileData);
            }
        }

        public void TrackFileData(string relativePath, string newVersionId, Type assetType, long assetId ,long fileSizeKb, DateTime createDate, DateTime lastUsedDate)
        {
            var existedData = GetFileData(relativePath);
            if (existedData != null)
            {
                existedData.Version = newVersionId;
                existedData.SizeKb = fileSizeKb;
                existedData.DownloadedDateUTC = createDate;
                existedData.LastUsedDateUTC = lastUsedDate;
            }
            else
            {
                var entityName = _cacheSettings[assetType].UnifiedAssetTypeName;
                var fileData = new FileData(newVersionId, relativePath, createDate, lastUsedDate, 0,
                    entityName, assetId, fileSizeKb);
                if (!AddData(fileData))
                {
                    Debug.LogWarning($"Bridge: Failed to add file data to cache: {fileData.AssetTypeName}:{fileData.AssetId}");
                }
            }
        }

        public void IncrementUsedCount(string relativePath)
        {
            var data = GetFileData(relativePath);
            if (data == null)
            {
                Debug.LogWarning($"Trying to increment used count for not registered asset. Asset file path: {relativePath}");
                return;
            }
            data.UsingCount++;
        }

        public FileData GetFileData(string relativePath)
        {
            return FileDatas.FirstOrDefault(link => link.Path == relativePath);
        }

        public void ForgetFileData(FileData fileData)
        {
            if (fileData == null)
            {
                Debug.LogWarning($"Bridge: argument null exc in {typeof(CachedFilesData)}.{nameof(ForgetFileData)}");
                return;
            }
            
            var assetTypeName = fileData.AssetTypeName;
            var assetId = fileData.AssetId;
            if (!_fileDatas.TryRemove(GetKey(fileData), out fileData))
            {
                Debug.LogWarning($"Bridge: Failed to remove cache file data: {assetTypeName}:{assetId}");
            }
        }

        public void Reset()
        {
            _fileDatas = new ConcurrentDictionary<string, FileData>();
        }
        
        private bool AddData(FileData fileData)
        {
            return _fileDatas.TryAdd(GetKey(fileData), fileData);
        }

        private string GetKey(FileData fileData)
        {
            return fileData.Path;
        }
    }
}