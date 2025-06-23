using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;
using Bridge.Services.Advertising;
using UnityEngine;

namespace Bridge.Services.AssetService
{
    internal interface IAssetService
    {
        Task<GetAssetResult> GetAssetAsync(IFilesAttachedEntity target, FileInfo fileInfo, bool cacheFile, CancellationToken cancellationToken);

        Task<FetchResult> Fetch(IFilesAttachedEntity target, FileInfo fileInfo, CancellationToken cancellationToken);

        void CancelAllFileLoadingProcesses();

        Task<Result<Texture2D>> GetImageAsync(string key, bool cache, CancellationToken cancellationToken);

        Task<Result> FetchImageAsync(string key, CancellationToken token);

        Result<Texture2D> GetImageFromCache(string key);
        
        Result<Texture2D> GetImageFromCache(IThumbnailOwner thumbnailOwner, FileInfo targetImage);
       
        bool HasImageCached(string key);
        
        Task<FileUploadResult> UploadFileAsync(FileInfo fileInfo, CancellationToken cancellationToken);

        Task<FileUploadResult[]> UploadFilesAsync(FileInfo[] fileInfos, CancellationToken cancellationToken);

        Task<ConvertingUrlResponse> InitConverting(string fileExtension);
        
        string GetUrl(IFilesAttachedEntity model, FileInfo fileInfo);
        
        Task<BannerTextureResult> GetBanner(SongAdData songData, bool cache);
      
        Task<Result> FetchAssets(PreFetchPack pack, int maxConcurrentRequests, Action<float> progressCallback, CancellationToken token = default);

        Task<ChatMessageFilesResult> GetMessageFiles(ChatMessage chatMessage, CancellationToken token = default);
    }
}
