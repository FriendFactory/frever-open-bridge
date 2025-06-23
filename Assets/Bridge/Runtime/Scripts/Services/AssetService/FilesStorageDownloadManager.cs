using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;
using UnityEngine;

namespace Bridge.Services.AssetService
{
    /// <summary>
    /// Responsible for loading files by key(not attached to the entities)
    /// </summary>
    internal sealed class FilesStorageDownloadManager
    {
        private const string END_POINT = "StorageFile";

        private readonly IHostProvider _hostProvider;
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;
        private readonly IStorageFileCache _assetsCache;
        private readonly List<DownloadingRequest> _runningRequests = new List<DownloadingRequest>();

        public FilesStorageDownloadManager(IHostProvider hostProvider, IRequestHelper requestHelper, ISerializer serializer, IStorageFileCache assetsCache)
        {
            _hostProvider = hostProvider;
            _requestHelper = requestHelper;
            _serializer = serializer;
            _assetsCache = assetsCache;
        }

        public async Task<Result<Texture2D>> DownloadImageAsync(string key, bool cache, CancellationToken token)
        {
            var actualVersionResp = await GetLatestFileVersion(key, token);
            if (actualVersionResp.IsError)
            {
                return Result<Texture2D>.Error(actualVersionResp.ErrorMessage);
            }

            var actualInfo = actualVersionResp.Model;
            if (_assetsCache.HasInCache(actualInfo))
            {
                var bytes = await _assetsCache.GetRawDataAsync(actualInfo, token);
                return Result<Texture2D>.Success(CreateTexture(bytes));
            }

            var isOriginReq = false;
            var runningRequest = _runningRequests.FirstOrDefault(x => x.Key == key && x.FileVersion == actualInfo.Version);
            if (runningRequest != null && !runningRequest.IsFinished)
            {
                while (!runningRequest.IsFinished && !token.IsCancellationRequested)
                {
                    await Task.Delay(33, token);
                }
                if (runningRequest.IsCancelled && !token.IsCancellationRequested)
                {
                    return await DownloadImageAsync(key, cache, token); // try again
                }
            }
            else
            {
                isOriginReq = true;
                runningRequest = new DownloadingRequest(GetDownloadRequest, actualInfo, token);
                _runningRequests.Add(runningRequest);
                await runningRequest.FetchAsync();
            }

            if (isOriginReq)
            {
                _runningRequests.Remove(runningRequest);
            }
            
            if (!runningRequest.IsSuccess)
            {
                return Result<Texture2D>.Error(runningRequest.ErrorMessage);
            }

            if (cache)
            {
                await SaveFileToCache(actualInfo, runningRequest.RawData);
            }

            return Result<Texture2D>.Success(runningRequest.Texture2D);
        }

        public async Task<Result> FetchImageAsync(string key, CancellationToken token)
        {
            var actualVersionResp = await GetLatestFileVersion(key, token);
            if (actualVersionResp.IsError)
            {
                return Result<Texture2D>.Error(actualVersionResp.ErrorMessage);
            }
            
            var actualInfo = actualVersionResp.Model;
            if (_assetsCache.HasInCache(actualInfo))
            {
                return new SuccessResult();
            }

            var isOriginReq = false;
            var runningRequest = _runningRequests.FirstOrDefault(x => x.Key == key && x.FileVersion == actualInfo.Version);
            if (runningRequest != null && !runningRequest.IsFinished)
            {
                while (!runningRequest.IsFinished && !token.IsCancellationRequested)
                {
                    await Task.Delay(33, token);
                }
                if (runningRequest.IsCancelled && !token.IsCancellationRequested)
                {
                    return await FetchImageAsync(key, token); // try again
                }
            }
            else
            {
                isOriginReq = true;
                runningRequest = new DownloadingRequest(GetDownloadRequest, actualInfo, token);
                _runningRequests.Add(runningRequest);
                await runningRequest.FetchAsync();
            }

            if (isOriginReq)
            {
                _runningRequests.Remove(runningRequest);
            }
            
            if (!runningRequest.IsSuccess)
            {
                return Result<Texture2D>.Error(runningRequest.ErrorMessage);
            }

            await SaveFileToCache(actualInfo, runningRequest.RawData);
            return new SuccessResult();
        }
        
        public Result<Texture2D> GetImageFromCache(string key)
        {
            if (!HasImageCached(key))
            {
                return Result<Texture2D>.Error($"Requested file is not cached. File key: {key}");
            }

            var bytesResult = _assetsCache.GetImageBytesFromCache(key);
            var texture = CreateTexture(bytesResult.Model);
            return Result<Texture2D>.Success(texture);
        }
        
        public bool HasImageCached(string key)
        {
            return _assetsCache.HasInCache(key);
        }

        private async Task SaveFileToCache(StorageFileInfo storageFileInfo, byte[] bytes)
        {
            _assetsCache.DeleteAllVersions(storageFileInfo);
            await _assetsCache.SaveToCacheAsync(storageFileInfo, bytes);
        }

        private async Task<Result<StorageFileInfo>> GetLatestFileVersion(string key, CancellationToken token)
        {
            var url =  Extensions.CombineUrls(_hostProvider.ClientServerHost, $"{END_POINT}");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var body = _serializer.SerializeToJson(new
            {
                Key = key
            });
            req.AddJsonContent(body);
            var resp = await req.GetHTTPResponseAsync(token: token);
            if (!resp.IsSuccess)
            {
                return Result<StorageFileInfo>.Error(resp.DataAsText);
            }

            var model = _serializer.DeserializeJson<StorageFileInfo>(resp.DataAsText);
            return Result<StorageFileInfo>.Success(model);
        }

        private HTTPRequest GetDownloadRequest(StorageFileInfo storageFileInfo)
        {
            var url = Extensions.CombineUrls(_hostProvider.AssetSeverHost, $"Cdn/{END_POINT}/{storageFileInfo.Version}/{storageFileInfo.Key}");
            return _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
        }

        private Texture2D CreateTexture(byte[] bytes)
        {
            var texture2d = new Texture2D(1, 1);
            texture2d.LoadImage(bytes, true);
            return texture2d;
        }
        
        private sealed class DownloadingRequest
        {
            private readonly Func<StorageFileInfo, HTTPRequest> _createHttpRequestDelegate;
            private readonly CancellationToken _cancellationToken;
            private HTTPResponse _httpResponse;
            private byte[] _rawData;
            private Texture2D _texture2D;

            public readonly StorageFileInfo StorageFileInfo;

            public string Key => StorageFileInfo.Key;
            public string FileVersion => StorageFileInfo.Version;
           
            public bool IsFinished { get; private set; }
            public bool IsSuccess { get; private set; }
            public bool IsCancelled { get; private set; }
            public bool IsError { get; private set; }
            public string ErrorMessage { get; private set; }

            public byte[] RawData => _rawData ?? (_rawData = _httpResponse.Data);

            public Texture2D Texture2D
            {
                get
                {
                    if (_texture2D == null)
                    {
                        _texture2D = _httpResponse.DataAsTexture2D;
                    }

                    return _texture2D;
                }
            }

            public DownloadingRequest(Func<StorageFileInfo, HTTPRequest> createHttpRequestDelegate, StorageFileInfo storageFileInfo, CancellationToken cancellationToken)
            {
                _createHttpRequestDelegate = createHttpRequestDelegate;
                StorageFileInfo = storageFileInfo;
                _cancellationToken = cancellationToken;
            }

            public async Task FetchAsync()
            {
                try
                {
                    var request = _createHttpRequestDelegate(StorageFileInfo);
                    _httpResponse = await request.GetHTTPResponseAsync(_cancellationToken);
                    IsFinished = true;

                    if (_cancellationToken.IsCancellationRequested)
                    {
                        IsCancelled = true;
                        return;
                    }
                
                    if (_httpResponse.IsSuccess)
                    {
                        IsSuccess = true;
                        return;
                    }

                    IsError = true;
                    ErrorMessage = _httpResponse.DataAsText;
                }
                catch (Exception e)
                {
                    IsError = true;
                    ErrorMessage = e.Message;
                }
            }
        }
    }
    
    internal struct StorageFileInfo
    {
        public string Key { get; set; }

        public string Version { get; set; }
    }
}