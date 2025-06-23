using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.StartPack.Metadata;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private IStartPackService _startPackService;
        
        public async Task<StartPackResult<MetadataStartPack>> GetMetadataStartPackAsync( CancellationToken token = default)
        {
            try
            {
                return await _startPackService.GetMetadataStartPackAsync(token);
            }
            catch (OperationCanceledException)
            {
                return StartPackResult<MetadataStartPack>.Canceled();
            }
        }

        public async Task<StartPackResult<DefaultUserAssets>> GetUserStartupAssetsDataAsync(IDictionary<string, string> headers, CancellationToken token = default)
        {
            try
            {
                return await _startPackService.GetUserStartupAssetsDataAsync(headers, token);
            }
            catch (OperationCanceledException)
            {
                return StartPackResult<DefaultUserAssets>.Canceled();
            }
        }
        
        public async Task<Result> FetchStartPackAssets(int maxConcurrentRequests, Action<float> progressCallback, CancellationToken token)
        {
            var result = await GetPreFetchStartPackAsync(token);
            if (!result.IsSuccess) return result;
            var fetchPack = result.Pack;
            return await _assetService.FetchAssets(fetchPack, maxConcurrentRequests, progressCallback, token);
        }
        
        private async Task<StartPackResult<PreFetchPack>> GetPreFetchStartPackAsync(CancellationToken token)
        {
            try
            {
                return await _startPackService.GetPreFetchStartPackAsync(token);
            }
            catch (OperationCanceledException)
            {
                return StartPackResult<PreFetchPack>.Canceled();
            }
        }

    }
}