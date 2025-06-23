using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;
using Bridge.Services.AssetService.Caching.AssetReaders;

namespace Bridge.Services.AssetService
{
    /// <summary>
    /// Responsible for loading files linked to entities (which has ID and different file settings)
    /// </summary>
    internal sealed class EntityFileDownloadManager
    {
        private const int DOWNLOAD_FROM_SERVER_WAIT_INTERVAL_MS = 15;
        private const int DOWNLOAD_FROM_LOCAL_STORAGE_WAIT_INTERVAL_MS = 3;
        
        private readonly HashSet<AssetLoadingJob> _runningJobs = new HashSet<AssetLoadingJob>();
        private readonly AssetsCache _assetsCache;
        private readonly AssetUrlProvider _assetUrlProvider;
        private readonly IRequestHelper _authService;
        private readonly AssetReaderProvider _assetReaderProvider;
        private readonly AssetDownloadRequestProvider _downloadRequestProvider;
        private readonly AssetFetchRequestProvider _fetchRequestProvider;

        public EntityFileDownloadManager(AssetsCache cache, IRequestHelper authService,
            AssetReaderProvider assetReaderProvider, AssetDownloadRequestProvider downloadRequestProvider,
            AssetFetchRequestProvider fetchRequestProvider, AssetUrlProvider assetUrlProvider)
        {
            _assetUrlProvider = assetUrlProvider;
            _assetsCache = cache;
            _authService = authService;
            _assetReaderProvider = assetReaderProvider;
            _downloadRequestProvider = downloadRequestProvider;
            _fetchRequestProvider = fetchRequestProvider;
        }

        public async Task<FileResult> Download(DownloadReq req, CancellationToken cancellationToken)
        {
            var modelToDownload = GetModelToDownload(req.Model, req.FileInfo);
            var fileInfoToDownload = req.FileInfo.State == FileState.ShouldBeCopiedFromSource
                ? modelToDownload.Files.First()
                : req.FileInfo;
            
            var validationResult = Validate(modelToDownload, fileInfoToDownload);
            if (!validationResult.IsSuccess)
            {
                if(req.PrefetchOnly)
                    return new FetchResult(validationResult.FailedReasonMessage);
                return new GetAssetResult(validationResult.FailedReasonMessage);
            }

            var executed = FindExecutedJob(modelToDownload, fileInfoToDownload, req.PrefetchOnly);
            var isPrimaryRequest = false;
            if (executed == null || executed.IsCancelled)
            {
                isPrimaryRequest = true;
                var modelToDownloadWasRedirected = modelToDownload != req.Model;
                if (modelToDownloadWasRedirected)
                {
                    req = new DownloadReq(modelToDownload, fileInfoToDownload, req.PrefetchOnly, req.StreamingType,
                        req.CacheFile);
                }
                executed = ExecuteNewJob(req, cancellationToken);
            }

            await WaitJob(executed, cancellationToken);
            
            if (isPrimaryRequest)
            {
                RemoveJob(executed);
            }

            if (isPrimaryRequest || !executed.Result.IsRequestCanceled) return executed.Result;
            
            //if we have 2 parallel requests from client and only one requested to be cancelled,
            //we still need download for another
            if (!cancellationToken.IsCancellationRequested)
            {
                return await Download(req, cancellationToken);
            }

            return executed.Result;
        }

        public void CancelAllLoadings()
        {
            if (_runningJobs.Count == 0) return;

            foreach (var job in _runningJobs)
            {
                job.Cancel();
            }
            
            _runningJobs.Clear();
        }

        private IFilesAttachedEntity GetModelToDownload(IFilesAttachedEntity model, FileInfo fileInfo)
        {
            if (fileInfo.State != FileState.ShouldBeCopiedFromSource)
                return model;

            var sourceData = fileInfo.Source.CopyFrom;
            var sourceModel = (IFilesAttachedEntity)Activator.CreateInstance(model.GetType());
            sourceModel.Id = sourceData.Id;
            var sourceFileInfo = new FileInfo(fileInfo.FileType, sourceData.Version, fileInfo.Extension, fileInfo.Resolution);
            sourceFileInfo.TagAsSyncedWithServer();
            sourceModel.Files = new List<FileInfo> {sourceFileInfo};
            return sourceModel;
        }

        private void RemoveJob(AssetLoadingJob job)
        {
            if (!_runningJobs.Contains(job)) return;
            _runningJobs.Remove(job);
        }

        private AssetLoadingJob CreateLoadingJob(DownloadReq req, CancellationToken cancellationToken)
        {
            return new AssetLoadingJob(req, _assetReaderProvider, _downloadRequestProvider, _fetchRequestProvider, _assetsCache, _authService,
                _assetUrlProvider, cancellationToken);
        }

        private ValidationResult Validate(IFilesAttachedEntity model, FileInfo fileInfo)
        {
            var targetFileInfo = model.Files.FirstOrDefault(x =>
                x.FileType == fileInfo.FileType && x.Resolution == fileInfo.Resolution);

            if (targetFileInfo == null)
                return new ValidationResult()
                {
                    FailedReasonMessage = $"{model.GetType().Name} {model.Id} has no file info for target file"
                };

            if (targetFileInfo.State == FileState.SyncedWithServer && string.IsNullOrEmpty(targetFileInfo.Version))
                return new ValidationResult()
                {
                    FailedReasonMessage = $"{model.GetType().Name} {model.Id} has no empty or null version. Version must be provided"
                };

            return new ValidationResult()
            {
                IsSuccess = true
            };
        }

        private AssetLoadingJob ExecuteNewJob(DownloadReq req, CancellationToken cancellationToken)
        {
            var job = CreateLoadingJob(req, cancellationToken);
            ExecuteJob(job);
            return job;
        }

        private void ExecuteJob(AssetLoadingJob loadingJob)
        {
            try
            {
                _runningJobs.Add(loadingJob);
#pragma warning disable CS4014
                loadingJob.Run();
#pragma warning restore CS4014
            }
            catch (Exception)
            {
                RemoveJob(loadingJob);
                throw;
            }
        }

        private AssetLoadingJob FindExecutedJob(IFilesAttachedEntity model, FileInfo fileInfo, bool prefetchOnly)
        {
           return _runningJobs.FirstOrDefault(x => x.Target.Id == model.Id && x.FileInfo == fileInfo
               && x.IsOnlyFetch == prefetchOnly && x.Target.GetType() == model.GetType());
        }

        private async Task WaitJob(AssetLoadingJob job, CancellationToken cancellationToken)
        {
            var waitingInterval = job.ReadingFileFromLocalStorage
                ? DOWNLOAD_FROM_LOCAL_STORAGE_WAIT_INTERVAL_MS
                : DOWNLOAD_FROM_SERVER_WAIT_INTERVAL_MS;

            try
            {
                while (!job.IsCompleted && !job.IsCancelled && !cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(waitingInterval, cancellationToken);
                }
            }
            catch (Exception)
            {
                RemoveJob(job);
                throw;
            }
        }

        private struct ValidationResult
        {
            public bool IsSuccess;
            public string FailedReasonMessage;
        }
    }
}