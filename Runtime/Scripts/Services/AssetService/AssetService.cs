using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Models.AsseManager;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.Advertising;
using Bridge.Services.AssetService.Caching;
using Bridge.Services.AssetService.Caching.AssetReaders;
using Bridge.Services.AssetService.Caching.CachePathGeneration;
using Newtonsoft.Json;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.Services.AssetService
{
    internal sealed class AssetService : IAssetService
    {
        private const int PARALLEL_UPLOADING_PROCESSES_LIMIT = 8;
        
        private static readonly IReadOnlyDictionary<Type, StreamingType> StreamingTypes =
            new Dictionary<Type, StreamingType>()
            {
                {typeof(VideoClip), StreamingType.FromDisk},
                {typeof(Template), StreamingType.FromRam}
            };

        private readonly EntityFileDownloadManager _entityFileDownloadManager;
        private readonly FilesStorageDownloadManager _filesStorageDownloadManager;
        private readonly AssetUrlProvider _assetUrlProvider;
        private readonly FileUploader[] _fileUploaders;
        private readonly IRequestHelper _requestHelper;
        private readonly AssetsCache _cache;
        private readonly string _serverUrl;
        private readonly Platform _targetPlatform;

        private ISongsAdsCache SongsAdCache => _cache;

        public AssetService(string hostUrl, IRequestHelper requestHelper, IHostProvider hostProvider, AssetReaderProvider assetReaderProvider,
            AssetDownloadRequestProvider assetDownloadRequestProvider, AssetsCache assetsCache, IStorageFileCache storageFileCache, Platform targetPlatform, ISerializer serializer)
        {
            _serverUrl = hostUrl;
            _requestHelper = requestHelper;
            _assetUrlProvider = new AssetUrlProvider(hostUrl, Config.Settings);
            var fetchRequestProvider = new AssetFetchRequestProvider(assetsCache, _requestHelper);
            _entityFileDownloadManager = new EntityFileDownloadManager(assetsCache, requestHelper, assetReaderProvider,
                assetDownloadRequestProvider, fetchRequestProvider, _assetUrlProvider);
            _filesStorageDownloadManager = new FilesStorageDownloadManager(hostProvider, requestHelper, serializer, storageFileCache);
            _fileUploaders = new FileUploader[]
            {
                new FilesFromDiskUploader(_serverUrl, requestHelper),
                new FilesFromMemoryUploader(_serverUrl, requestHelper)
            };
            _cache = assetsCache;
            _targetPlatform = targetPlatform;
        }

        public async Task<FetchResult> Fetch(IFilesAttachedEntity model, FileInfo fileInfo, CancellationToken cancellationToken)
        {
            try
            {
                var streamingType = GetStreamingTypeIfAssetStreamable(model);
                var req = new DownloadReq(model, fileInfo, true, streamingType, true);
                return await DownloadAsset(req, cancellationToken) as FetchResult;
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? new CanceledFetchResult()
                    : new FetchResult(e.Message);
            }
        }

        public async Task<Result<Texture2D>> GetImageAsync(string key, bool cache, CancellationToken cancellationToken)
        {
            try
            {
                return await _filesStorageDownloadManager.DownloadImageAsync(key, cache, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<Texture2D>.Cancelled()
                    : Result<Texture2D>.Error(e.Message);
            }
        }

        public async Task<Result> FetchImageAsync(string key, CancellationToken token)
        {
            try
            {
                return await _filesStorageDownloadManager.FetchImageAsync(key, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? new CanceledResult()
                    : (Result) new ErrorResult(e.Message);
            }
        }

        public Result<Texture2D> GetImageFromCache(IThumbnailOwner thumbnailOwner, FileInfo targetImage)
        {
            try
            {
                var cachedFileData = _cache.GetFileData(thumbnailOwner, targetImage);
                if (cachedFileData == null)
                {
                    return Result<Texture2D>.Error("File is not in the cache");
                }

                var fullPath = _cache.GetFullPath(cachedFileData);
                var bytes = File.ReadAllBytes(fullPath);
                var texture2d = new Texture2D(1, 1);
                texture2d.LoadImage(bytes, true);
                return Result<Texture2D>.Success(texture2d);
            }
            catch (Exception e)
            {
                return Result<Texture2D>.Error(e.Message);
            }
        }

        public void CancelAllFileLoadingProcesses()
        {
            _entityFileDownloadManager.CancelAllLoadings();
        }

        public Result<Texture2D> GetImageFromCache(string key)
        {
            return _filesStorageDownloadManager.GetImageFromCache(key);
        }

        public bool HasImageCached(string key)
        {
            return _filesStorageDownloadManager.HasImageCached(key);
        }

        public async Task<GetAssetResult> GetAssetAsync(IFilesAttachedEntity model, FileInfo fileInfo, bool cacheFile,
            CancellationToken cancellationToken)
        {
            try
            {
                var streamingType = GetStreamingTypeIfAssetStreamable(model);
                var req = new DownloadReq(model, fileInfo, false, streamingType, cacheFile);
                return await DownloadAsset(req, cancellationToken) as
                    GetAssetResult;
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? new CanceledGetAssetResult()
                    : new GetAssetResult(e.Message);
            }
        }

        public Task<FileUploadResult> UploadFileAsync(FileInfo fileInfo, CancellationToken cancellationToken)
        {
            return UploadFileInternal(fileInfo, cancellationToken);
        }

        public async Task<FileUploadResult[]> UploadFilesAsync(FileInfo[] fileInfos,
            CancellationToken cancellationToken)
        {
            //preventing twice uploading of the same file, if 2+ FileInfo reference to the same local file
            var fileInfoWithUniqueFiles = SelectUniqueFiles(fileInfos, out var duplicatedFiles);

            var totalRequiredDeploys = fileInfoWithUniqueFiles.Length;
            var finishedTasks = new List<Task<FileUploadResult>>();
            var runningTasks = new List<Task<FileUploadResult>>();
            var nextFileInfoIndex = 0;
            //keep constant count of active parallel upload requests. If anyone is finished, run the next one
            do
            {
                var tasksToAddCount = PARALLEL_UPLOADING_PROCESSES_LIMIT - runningTasks.Count;
                var lastIndex = Mathf.Min(nextFileInfoIndex + tasksToAddCount, totalRequiredDeploys) - 1;
                for (var fileInfoIndex = nextFileInfoIndex; fileInfoIndex <= lastIndex; fileInfoIndex++)
                {
                    runningTasks.Add(UploadFileAsync(fileInfoWithUniqueFiles[fileInfoIndex], cancellationToken));
                    nextFileInfoIndex++;
                }

                await Task.WhenAny(runningTasks);
                
                var completed = runningTasks.Where(x => x.IsCompleted || x.IsCanceled).ToArray();
                finishedTasks.AddRange(completed);
                foreach (var task in completed)
                {
                    runningTasks.Remove(task);
                }
            
                if(cancellationToken.IsCancellationRequested)
                {
                    return finishedTasks.Select(x=>x.Result).ToArray();
                }
            } while (totalRequiredDeploys > finishedTasks.Count);

            var uniqueFileResults = finishedTasks.Select(x => x.Result).ToArray();
            var outputResults = new List<FileUploadResult>(uniqueFileResults);

            //apply uploading results for file info referenced for the same file
            foreach (var duplicatedFile in duplicatedFiles)
            {
                var deployedSimilarFileResult =
                    uniqueFileResults.First(x => x.FileInfo.FilePath == duplicatedFile.FilePath);
                FileUploadResult result;
                if (deployedSimilarFileResult.IsError)
                {
                    result = new FileUploadResult(deployedSimilarFileResult.ErrorMessage);
                }
                else
                {
                    duplicatedFile.Source.UploadId = deployedSimilarFileResult.UploadId;
                    result = new FileUploadResult(duplicatedFile, deployedSimilarFileResult.UploadId);
                }

                outputResults.Add(result);
            }

            return outputResults.ToArray();
        }

        public async Task<ConvertingUrlResponse> InitConverting(string fileExtension)
        {
            var baseUri = new Uri(_serverUrl);
            var uri = new Uri(baseUri, $"File/PreConversionUrl/{fileExtension}");

            var request = _requestHelper.CreateRequest(uri, HTTPMethods.Get, true, false);
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new ConvertingUrlResponse(resp.DataAsText);

            var respJson = resp.DataAsText;
            var respModel = JsonConvert.DeserializeObject<InitConversionModel>(respJson);
            return new ConvertingUrlResponse(respModel.UploadUrl, respModel.CheckFileConvertedUrl,
                respModel.ConvertedFileUrl,
                respModel.TargetFileExtension,
                respModel.UploadId);
        }

        public string GetUrl(IFilesAttachedEntity model, FileInfo fileInfo)
        {
            return _assetUrlProvider.GetUrl(model, fileInfo);
        }
        
        public async Task<BannerTextureResult> GetBanner(SongAdData songData, bool cache)
        {
            try
            {
                return await GetBannerInternal(songData, cache);
            }
            catch (Exception e)
            {
                return new BannerTextureResult(e.Message);
            }
        }

        public async Task<Result> FetchAssets(PreFetchPack pack, int maxConcurrentRequests, Action<float> progressCallback, CancellationToken token = default)
        {
            try
            {
                return await FetchStartPackAssetsInternal(pack, maxConcurrentRequests, progressCallback,token);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return new ErrorResult(e.Message);
            }
        }

        public async Task<ChatMessageFilesResult> GetMessageFiles(ChatMessage chatMessage, CancellationToken token = default)
        {
            try
            {
                var downloadedFiles = new List<GetAssetResult>();
                foreach (var fileInfo in chatMessage.Files)
                {
                    var result = await GetAssetAsync(chatMessage, fileInfo, true, token);
                    downloadedFiles.Add(result);
                    if (token.IsCancellationRequested) return ChatMessageFilesResult.Cancelled();
                }

                var images = downloadedFiles.Where(x => x.IsSuccess).Select(x => x.Object as Texture2D).ToArray();
                return downloadedFiles.Any(x => x.IsError) 
                    ? ChatMessageFilesResult.Error(downloadedFiles.First(x => x.IsError).ErrorMessage, images) 
                    : ChatMessageFilesResult.Success(images);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return ChatMessageFilesResult.Cancelled();
                return ChatMessageFilesResult.Error(e.Message);
            }
        }

        private async Task<Result> FetchStartPackAssetsInternal(PreFetchPack pack, int maxConcurrentRequests,  Action<float> progressCallback, CancellationToken token)
        {
            var assetsToPrefetch = CollectAssetsToPrefetch(pack);

            var totalLoaded = 0;
            var active = 0;
            var tasks = new Task<FetchResult>[assetsToPrefetch.Count];
            for (var i = 0; i < assetsToPrefetch.Count; i++)
            {
                // Don't allow more than x concurrent downloading requests
                while (active >= maxConcurrentRequests)
                {
                    await Task.Delay(50, token);
                }

                var asset = assetsToPrefetch[i];
                active++;
                var fileInfo = asset.Files.FirstOrDefault(file => file.Platform == _targetPlatform) ?? asset.Files.First();
                tasks[i] = FetchInternalWithCallback(asset, fileInfo, token, () =>
                {
                    active--;
                    totalLoaded++;
                    var progress = totalLoaded / (float)assetsToPrefetch.Count();
                    progressCallback?.Invoke(progress);
                });
            }

            await Task.WhenAll(tasks);

            if (tasks.Any(x => x.Result.IsError))
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat("Succeed loaded: {0}", tasks.Count(x=>x.Result.IsSuccess));
                foreach (var failedFetching in tasks.Where(x=>x.Result.IsError))
                {
                    var entityIndex = Array.FindIndex(tasks, val => val.Equals(failedFetching));
                    var entity = assetsToPrefetch[entityIndex];
                    errorMessage.AppendLine();
                    errorMessage.AppendFormat("Failed to load {0} Id {1}. Reason: {2}", entity.GetType().Name, entity.Id, failedFetching.Result.ErrorMessage);
                }

                return new ErrorResult(errorMessage.ToString());
            }
            
            return new SuccessResult();
        }

        private static List<IFilesAttachedEntity> CollectAssetsToPrefetch(PreFetchPack pack)
        {
            var assetsToPrefetch = new List<IFilesAttachedEntity>();
            if (pack.BodyAnimations != null) assetsToPrefetch.AddRange(pack.BodyAnimations.Select(Convert<BodyAnimationInfo>));
            if (pack.Vfxs != null) assetsToPrefetch.AddRange(pack.Vfxs.Select(Convert<VfxInfo>));
            if (pack.UmaBundles != null) assetsToPrefetch.AddRange(pack.UmaBundles.Select(Convert<UmaBundleFullInfo>));
            if (pack.CameraFilterVariants != null)
                assetsToPrefetch.AddRange(pack.CameraFilterVariants.Select(Convert<CameraFilterVariantInfo>));
            if (pack.SetLocationBundles != null)
                assetsToPrefetch.AddRange(pack.SetLocationBundles.Select(Convert<SetLocationBundleInfo>));
            return assetsToPrefetch;
        }

        private async Task<FetchResult> FetchInternalWithCallback(IFilesAttachedEntity model, FileInfo fileInfo, CancellationToken cancellationToken, Action callback)
        {
            var result = await Fetch(model, fileInfo, cancellationToken);
            callback?.Invoke();
            return result;
        }

        private static T Convert<T>(IFilesAttachedEntity entity)where T: class, IFilesAttachedEntity, new()
        {
            return new T
            {
                Id = entity.Id,
                Files = entity.Files
            };
        }

        private Task<FileResult> DownloadAsset(DownloadReq req, CancellationToken cancellationToken)
        {
            return _entityFileDownloadManager.Download(req, cancellationToken);
        }

        private Task<FileUploadResult> UploadFileInternal(FileInfo fileInfo, CancellationToken cancellationToken)
        {
            var uploader = _fileUploaders.First(x => x.TargetFilePlacingType == fileInfo.PlacingType);
            return uploader.UploadAsync(fileInfo, cancellationToken);
        }

        private FileInfo[] SelectUniqueFiles(FileInfo[] target, out FileInfo[] duplicated)
        {
            var filesFromRam = target.Where(x => x.PlacingType == FilePlacingType.InMemory);
            var uniqueLocalFiles = target.Where(x => x.FilePath != null).DistinctBy(x => x.FilePath).ToArray();
            var output = new List<FileInfo>();
            output.AddRange(filesFromRam);
            output.AddRange(uniqueLocalFiles);

            if (output.Count == target.Length)
            {
                duplicated = Array.Empty<FileInfo>();
                return output.ToArray();
            }

            var duplicatedList = new List<FileInfo>();
            foreach (var fileInfo in target)
            {
                if (uniqueLocalFiles.Any(x => ReferenceEquals(x, fileInfo)))
                    continue;
                duplicatedList.Add(fileInfo);
            }

            duplicated = duplicatedList.ToArray();
            return output.ToArray();
        }

        private StreamingType? GetStreamingTypeIfAssetStreamable(IFilesAttachedEntity model)
        {
            if (StreamingTypes.TryGetValue(model.GetType(), out var streamingType))
            {
                return streamingType;
            }
            return null;
        }
        
        private async Task<BannerTextureResult> GetBannerInternal(SongAdData songData, bool cache)
        {
            if (SongsAdCache.HasCached(songData))
            {
                var bannerFromCache = await SongsAdCache.GetBanner(songData);
                return new BannerTextureResult(bannerFromCache);
            }

            var banner = await DownloadBanner(songData, !cache);
            if (cache)
            {
                SaveToCache(songData, banner);
            }
            return new BannerTextureResult(banner);
        }

        private async void SaveToCache(SongAdData songData, Texture2D banner)
        {
            await SongsAdCache.Cache(songData, banner);
        }

        private async Task<Texture2D> DownloadBanner(SongAdData songData, bool asNonReadable)
        {
            var req = _requestHelper.CreateRequest(songData.BannerUrl, HTTPMethods.Get, false, false);
            var bytes = await req.GetRawDataAsync();
            var texture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            texture.LoadImage(bytes, asNonReadable);
            return texture;
        }
        
        private class InitConversionModel
        {
            public string UploadUrl { get; set; }
            public string UploadId { get; set; }
            public string ConvertedFileUrl { get; set; }
            public string CheckFileConvertedUrl { get; set; }
            public string OriginalFileExtension { get; set; }
            public string TargetFileExtension { get; set; }
        }
    }
}