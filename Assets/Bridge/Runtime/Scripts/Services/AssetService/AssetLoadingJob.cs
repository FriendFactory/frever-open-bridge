using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;
using Bridge.Services.AssetService.Caching.AssetReaders;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.Services.AssetService
{
    /// <summary>
    /// Responsible for loading asset from remote or cache(if available)
    /// </summary>
    internal sealed class AssetLoadingJob
    {
        public readonly bool ReadingFileFromLocalStorage;
        
        private readonly DownloadReq _downloadReq;
        private readonly AssetsCache _cache;
        private readonly AssetReaderProvider _assetReaderProvider;
        private readonly AssetDownloadRequestProvider _downloadRequestProvider;
        private readonly AssetFetchRequestProvider _fetchRequestProvider;
        private readonly AssetUrlProvider _assetUrlProvider;
        private readonly IRequestHelper _authService;
        private bool _processing;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        private bool IsStreamedType => FileStreamingType.HasValue;
        private StreamingType? FileStreamingType => _downloadReq.StreamingType;
        
        public FileInfo FileInfo => _downloadReq.FileInfo;
        public bool IsOnlyFetch => _downloadReq.PrefetchOnly;
        public bool CacheFile => _downloadReq.CacheFile;
        public IFilesAttachedEntity Target => _downloadReq.Model;
        public bool IsCompleted { get; private set; }
        public bool IsCancelled { get; private set; }
        public FileResult Result { get; private set; }
        
        private FileState FileState => FileInfo.State;
        
        public AssetLoadingJob(DownloadReq req, AssetReaderProvider assetReaderProvider,
            AssetDownloadRequestProvider assetDownloadRequestProvider, AssetFetchRequestProvider fetchRequestProvider,
            AssetsCache assetsCache, IRequestHelper authService, AssetUrlProvider assetUrlProvider, CancellationToken cancellationToken)
        {
            _downloadReq = req;
            _cache = assetsCache;
            _authService = authService;
            _assetReaderProvider = assetReaderProvider;
            cancellationToken.Register(Cancel);
            _cancellationTokenSource = new CancellationTokenSource();
            _downloadRequestProvider = assetDownloadRequestProvider;
            _fetchRequestProvider = fetchRequestProvider;
            _assetUrlProvider = assetUrlProvider;
            ReadingFileFromLocalStorage = FileState == FileState.ModifiedLocally;
        }

        public async Task Run()
        {
            if (_processing) throw new Exception("Trying to start already started downloading process");

            _processing = true;
            
            var result = await RunInternal();
            if (IsCancelled)
            {
                SetCancelledResult();
            }
            else
            {
                Result = result;   
            }

            IsCompleted = true;
        }

        public void Cancel()
        {
            if (IsCancelled || IsCompleted) return;
            
            IsCancelled = true;
            SetCancelledResult();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void SetCancelledResult()
        {
            Result = IsOnlyFetch ? new CanceledFetchResult() : (FileResult)new CanceledGetAssetResult();
        }

        private async Task<FileResult> RunInternal()
        {
            if (FileState == FileState.ModifiedLocally)
            {
                return await ReadLocalModifiedFile();
            }

            var filePath = _cache.GetFilePath(Target.GetType(), Target.Id, FileInfo);
            
            if (_cache.HasInCache(Target, FileInfo))
            {
                var readFromCacheResult = await ReadFileFromCache(filePath);
                if (readFromCacheResult.IsSuccess || readFromCacheResult.IsRequestCanceled) return readFromCacheResult;
                Debug.LogWarning($"Failed to read from cache: {filePath}");
            }

            var url = _assetUrlProvider.GetUrl(Target, FileInfo);

            if (IsOnlyFetch)
            {
                var fetchReq = await FetchAsync(url);
                return !fetchReq.IsSuccess 
                    ? new FetchResult(fetchReq.ErrorMessage) 
                    : new FetchResult(filePath, FileStreamingType);
            }
            
            var isStreamedFromDisk = FileStreamingType == StreamingType.FromDisk;
            if (isStreamedFromDisk)
            {
                var fetchReq = await FetchAsync(url);
                return fetchReq.IsSuccess
                    ? new GetAssetResult(null, filePath, FileStreamingType)
                    : new GetAssetResult(fetchReq.ErrorMessage);
            }

            return await DownloadAndProvideAsset(url, filePath);
        }

        private async Task<FileResult> ReadLocalModifiedFile()
        {
            var filePath = FileInfo.FilePath;
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("Modified file must have path to modified file");
            }

            var fileExtension = Path.GetExtension(filePath);
            var reader = _assetReaderProvider.GetReader(fileExtension);
            await reader.Read(filePath, CancellationToken);
            return reader.ProvidesUnityObject 
                ? new GetAssetResult(reader.Asset, filePath, FileStreamingType) 
                : new GetAssetResult(reader.RawData, filePath, FileStreamingType);
        }

        private async Task<FileResult> ReadFileFromCache(string filePath)
        {
            if (IsOnlyFetch) return new FetchResult(filePath, FileStreamingType);
            
            if(IsStreamedType && FileStreamingType == StreamingType.FromDisk)
                return new GetAssetResult(null, filePath, FileStreamingType);
            
            return await _cache.GetFile(Target, FileInfo, FileStreamingType, CancellationToken);
        }

        private async Task<FetchAssetRequest> FetchAsync(string url)
        {
            var fetchReq = _fetchRequestProvider.GetFetchAssetRequest(); 
            await fetchReq.Fetch(Target, FileInfo, url, CancellationToken);
            return fetchReq;
        }

        private async Task<FileResult> DownloadAndProvideAsset(string url, string filePath)
        {
            using (var downloadRequest = _downloadRequestProvider.GetDownloadRequest(FileInfo.Extension))
            {
                await downloadRequest.DownloadAsset(url, _authService.Token.Value, CancellationToken);

                CancellationToken.ThrowIfCancellationRequested();

                if (!downloadRequest.IsSuccess)
                    return new GetAssetResult(downloadRequest.ErrorMessage);
                
                if (CacheFile)
                {
                    ExecuteAssetCachingInBackground(downloadRequest.AssetBytes);
                }

                return downloadRequest.AvailableOnlyRawData
                    ? new GetAssetResult(downloadRequest.AssetBytes, filePath, FileStreamingType, downloadRequest.AssetBytes) 
                    : new GetAssetResult(downloadRequest.Asset, filePath, FileStreamingType);
            }
        }

        private void ExecuteAssetCachingInBackground(byte[] fileRawData)
        {
            FileInfo.FileRawData = fileRawData;
            //run caching in another thread and don't wait result
#pragma warning disable CS4014
            Task.Run(()=>_cache.SaveToCacheAsync(Target, FileInfo), CancellationToken);
#pragma warning restore CS4014
        }
    }
}