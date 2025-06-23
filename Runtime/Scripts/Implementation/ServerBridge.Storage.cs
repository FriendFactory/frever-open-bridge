using System.Collections.Generic;
using System.Linq;
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
using Bridge.Models.Extensions;
using Bridge.Models.VideoServer;
using Bridge.Results;
using UnityEngine;
using Event = Bridge.Models.AsseManager.Event;
using Resolution = Bridge.Models.Common.Files.Resolution;
using CharacterInfo = Bridge.Models.ClientServer.Assets.CharacterInfo;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<GetAssetResult> GetAssetAsync<T>(T target, bool cacheFile, CancellationToken cancellationToken) 
            where T : IFilesAttachedEntity
        {
            if (!IsLoggedIn)
            {
                return Task.FromResult(new GetAssetResult("User not logged"));
            }

            var fileInfo = GetMainFileInfo(target);
            return GetAssetAsync(target, fileInfo, cacheFile, cancellationToken);
        }

        private FileInfo GetMainFileInfo(IFilesAttachedEntity target)
        {
            var mainFiles = target.Files.Where(x => x.FileType == FileType.MainFile).ToArray();

            //todo: remove when old ios assets will get platform in file info
            if (_runtimePlatform == RuntimePlatform.IPhonePlayer && mainFiles.All(x => x.Platform != Platform.iOS))
            {
                return mainFiles.First(x => x.Platform == null);
            }

            if (mainFiles.All(x => x.Platform == null))
            {
                return mainFiles.First();
            }

            return mainFiles.First(x => x.Platform == Platform);
        }

        public Task<GetAssetResult> GetAssetAsync<T>(T target, FileInfo fileInfo, bool cacheFile = true,
            CancellationToken cancellationToken = default) where T : IFilesAttachedEntity
        {
            return _assetService.GetAssetAsync(target, fileInfo, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetThumbnailAsync<T>(T target, Resolution resolution, bool cacheFile, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity
        {
            if (!IsLoggedIn)
            {
                return Task.FromResult(new GetAssetResult("User not logged"));
            }

            var fileInfo = target.Files.First(x => x.FileType == FileType.Thumbnail && x.Resolution == resolution);
            return GetThumbnailAsyncInternal(target, fileInfo, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetThumbnailAsync(LevelShortInfo target, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default)
        {
            var fileInfo = target.FirstEventFiles.First(x => x.Resolution == resolution);
            return GetEventThumbnailAsync(target.FirstEventId, fileInfo, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetThumbnailAsync(GroupShortInfo target, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default)
        {
            if (!target.MainCharacterId.HasValue)
            {
                return Task.FromResult(new GetAssetResult($"User does not have thumbnail"));
            }

            var fileInfo = target.MainCharacterFiles.FirstOrDefault(x => x.Resolution == resolution);
            if (fileInfo == null)
            {
                return Task.FromResult(
                    new GetAssetResult($"User does not have thumbnail with resolution: {resolution}"));
            }

            return GetCharacterThumbnailAsync(target.MainCharacterId.Value, fileInfo, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetThumbnailAsync(AssetInfo assetInfo, Resolution resolution, bool cacheFile = true,
            CancellationToken cancellationToken = default)
        {
            var assetModel = assetInfo.ToAssetModel();
            return GetThumbnailAsync(assetModel, resolution, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetThumbnailAsync(AssetStoreAssetType assetType, long assetId, List<FileInfo> filesInfos, Resolution resolution,
            bool cacheFile = true, CancellationToken cancellationToken = default)
        {
            var assetInfo = new AssetInfo { Id = assetId, AssetType = assetType, Files = filesInfos };
            return GetThumbnailAsync(assetInfo, resolution, cacheFile, cancellationToken);
        }

        public Result<Texture2D> GetThumbnailFromCacheImmediate<T>(T target, Resolution resolution) where T : IThumbnailOwner
        {
            var fileInfo = target.Files.First(x => x.FileType == FileType.Thumbnail && x.Resolution == resolution);
            if (!HasCached(target, fileInfo))
            {
                return Result<Texture2D>.Error("File is not in the cache");
            }

            return _assetService.GetImageFromCache(target, fileInfo);
        }
        
        public bool HasCached<T>(T entity, FileInfo targetFile) where T: IFilesAttachedEntity
        {
            return GetCachedFileData(entity, targetFile) != null;
        }

        public Task<Result<Texture2D>> GetImageAsync(string key, bool cache, CancellationToken token = default)
        {
            return _assetService.GetImageAsync(key, cache, token);
        }

        public Task<Result> FetchImageAsync(string key, CancellationToken token = default)
        {
            return _assetService.FetchImageAsync(key, token);
        }

        public void CancelAllFileLoadingProcesses()
        {
            _assetService.CancelAllFileLoadingProcesses();
        }

        public bool HasImageCached(string key)
        {
            return _assetService.HasImageCached(key);
        }

        public Result<Texture2D> GetImageFromCache(string key)
        {
            return _assetService.GetImageFromCache(key);
        }

        public Task<GetAssetResult> GetEventThumbnailAsync(long eventId, FileInfo fileInfo, bool cacheFile = true,
            CancellationToken cancellationToken = default)
        {
            var ev = new Event
            {
                Id = eventId,
                Files = new List<FileInfo> {fileInfo}
            };
            return GetThumbnailAsyncInternal(ev, fileInfo, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetCharacterThumbnailAsync(long characterId, FileInfo fileInfo, bool cacheFile = true,
            CancellationToken cancellationToken = default)
        {
            var character = new CharacterInfo
            {
                Id = characterId,
                Files = new List<FileInfo> { fileInfo }
            };
            return GetThumbnailAsync(character, fileInfo.Resolution.Value, cacheFile, cancellationToken);
        }

        public Task<GetAssetResult> GetCharacterBakedView(BakedView model, FileInfo fileInfo, bool cacheFile = true,
            CancellationToken cancellationToken = default)
        {
            return _assetService.GetAssetAsync(model, fileInfo, cacheFile, cancellationToken);
        }

        public Task<FetchResult> FetchMainAssetAsync<T>(T target, CancellationToken cancellationToken) 
            where T : IFilesAttachedEntity
        {
            var fileInfo = GetMainFileInfo(target);
            return _assetService.Fetch(target, fileInfo, cancellationToken);
        }

        public Task<FetchResult> FetchMainAssetAsync<T>(T target, FileInfo fileInfo, CancellationToken cancellationToken = default) where T : IFilesAttachedEntity
        {
            return _assetService.Fetch(target, fileInfo, cancellationToken);
        }

        public Task<FetchResult> FetchThumbnailAsync<T>(T target, Resolution resolution, CancellationToken cancellationToken = default) 
            where T : IFilesAttachedEntity
        {
            var fileInfo = target.Files.First(x => x.FileType == FileType.Thumbnail && x.Resolution == resolution);
            return _assetService.Fetch(target, fileInfo, cancellationToken);
        }

        public Task<ChatMessageFilesResult> GetMessageFiles(ChatMessage chatMessage, CancellationToken token = default)
        {
            return _assetService.GetMessageFiles(chatMessage, token);
        }

        public string GetTemplateVideoUrl(TemplateInfo template)
        {
            if (template.Files == null || template.Files.Count == 0) return string.Empty;
            return _assetService.GetUrl(template, template.Files.First());
        }

        public string GetTemplateVideoUrl(TemplateChallenge template)
        {
            if (template.Files == null || template.Files.Count == 0) return string.Empty;
            return _assetService.GetUrl(template, template.Files.First());
        }

        private Task<GetAssetResult> GetThumbnailAsyncInternal<T>(T target, FileInfo fileInfo, bool cacheFile, CancellationToken cancellationToken)
            where T: IFilesAttachedEntity
        {
            return _assetService.GetAssetAsync(target, fileInfo, cacheFile, cancellationToken);
        }
    }
}