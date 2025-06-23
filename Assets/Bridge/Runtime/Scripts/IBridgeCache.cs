using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge
{
    public interface IBridgeCache
    {
        bool IsCacheEmpty { get; }
        Task<Result> ClearCacheAsync(bool deleteUserLoginData = false, FFEnvironment? targetEnvironment = null, params CacheType[] cachesToClear);
        Task<Result> ClearAssetBundleAsync();
        Task<long> GetCacheSizeKb();
        Task<FileData> GetCachedFileDataAsync<T>(T target, FileInfo fileInfo) where T : IFilesAttachedEntity;
        FileData GetCachedFileData<T>(T target, FileInfo fileInfo) where T : IFilesAttachedEntity;
        IEnumerable<FileData> GetCachedFilesData();
        IEnumerable<FileData> GetCachedFilesData<T>() where T : IFilesAttachedEntity;
        string GetFilePath<T>(T target, FileInfo fileInfo) where T : IFilesAttachedEntity;
        string GetAssetTypeUnifiedName(IFilesAttachedEntity entity);
        string GetAssetTypeUnifiedName(Type entityType);
        string GetCachedFileFullPath(FileData fileData);
        Task DeleteAllFromCache<T>() where T : IFilesAttachedEntity;
        Task DeleteFromCache(FileData[] filesData);
        Task DeleteFromCache(FileData fileData);
        void DeleteTempFiles();
    }

    public enum CacheType
    {
        AssetFiles,
        FilesStoredByKey,
        TranscodingFiles
    }
}