using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Services.AssetService.Caching;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.Services.AssetService
{
    internal sealed class FetchAssetRequest
    {
        private const float TIME_OUT_MINUTES = 15;
        
        private readonly AssetsCache _assetsCache;
        private static HttpClient _client;

        public FetchAssetRequest(AssetsCache assetsCache, IRequestHelper requestHelper)
        {
            _assetsCache = assetsCache;

            if (_client == null)
            {
                _client = requestHelper.CreateClient(true);
                _client.Timeout = TimeSpan.FromMinutes(TIME_OUT_MINUTES);
            }
        }

        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        public async Task Fetch<T>(T model, FileInfo fileInfo, string url, CancellationToken cancellationToken = default)
            where T : IFilesAttachedEntity
        {
            try
            {
                await FetchInternal(model, fileInfo, url, cancellationToken);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        private async Task FetchInternal<T>(T model, FileInfo fileInfo, string url, CancellationToken cancellationToken)
            where T : IFilesAttachedEntity
        {
            using (var stream = await _client.GetStreamAsync(new Uri(url)))
            {
                await _assetsCache.SaveToCacheAsync(model, fileInfo, stream, cancellationToken);
            }

            IsSuccess = true;
        }
    }
}