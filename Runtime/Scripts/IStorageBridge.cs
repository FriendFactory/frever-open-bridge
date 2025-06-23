using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.ClientServer.Level;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Models.VideoServer;
using Bridge.Results;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace Bridge
{
    public interface IStorageBridge
    {
        void SetPlatform(RuntimePlatform platform);

        Task<GetAssetResult> GetAssetAsync<T>(T target, bool cacheFile = true, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity;  
        Task<GetAssetResult> GetAssetAsync<T>(T target, FileInfo fileInfo, bool cacheFile = true, CancellationToken cancellationToken = default) 
            where T : IFilesAttachedEntity;
        Task<GetAssetResult> GetThumbnailAsync<T>(T target, Resolution resolution, bool cacheFile = true, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity;
        Task<GetAssetResult> GetThumbnailAsync(LevelShortInfo target, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default);
        Task<GetAssetResult> GetThumbnailAsync(GroupShortInfo target, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default);
        Task<GetAssetResult> GetThumbnailAsync(AssetInfo assetInfo, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default);
        Task<GetAssetResult> GetThumbnailAsync(AssetStoreAssetType assetType, long assetId, List<FileInfo> filesInfos, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default);
        Result<Texture2D> GetThumbnailFromCacheImmediate<T>(T target, Resolution resolution) where T : IThumbnailOwner;
        bool HasCached<T>(T entity, FileInfo targetFile) where T : IFilesAttachedEntity;
        void CancelAllFileLoadingProcesses();
        Task<Result<Texture2D>> GetImageAsync(string key, bool cache = true, CancellationToken token = default);
        Task<Result> FetchImageAsync(string key, CancellationToken token = default);
        bool HasImageCached(string key);
        Result<Texture2D> GetImageFromCache(string key);
        Task<GetAssetResult> GetEventThumbnailAsync(long eventId, FileInfo fileInfo, bool cacheFile = true, CancellationToken cancellationToken = default);
        Task<GetAssetResult> GetCharacterThumbnailAsync(long characterId, FileInfo fileInfo, bool cacheFile = true, CancellationToken cancellationToken = default);
        Task<GetAssetResult> GetCharacterBakedView(BakedView model, FileInfo fileInfo, bool cacheFile = true,
            CancellationToken cancellationToken = default);
        Task<FetchResult> FetchMainAssetAsync<T>(T target, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity;
        Task<FetchResult> FetchMainAssetAsync<T>(T target, FileInfo fileInfo, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity;
        Task<FetchResult> FetchThumbnailAsync<T>(T target, Resolution resolution, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity;
        Task<ChatMessageFilesResult> GetMessageFiles(ChatMessage chatMessage, CancellationToken token = default);
        string GetTemplateVideoUrl(TemplateInfo template);
        string GetTemplateVideoUrl(TemplateChallenge template);
    }
}