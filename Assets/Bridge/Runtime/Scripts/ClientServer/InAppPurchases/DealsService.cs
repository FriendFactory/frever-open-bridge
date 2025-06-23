using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.InAppPurchases
{
    internal interface IDealsService
    {
        Task<ArrayResult<Deal>> GetDeals(CancellationToken token);
        Task<Result<AssetPurchaseResult>> PurchaseAsset(long assetOfferId);
        Task<Result<AssetPurchaseResult>> PurchaseSeasonLevel(int level);
    }
    
    internal sealed class DealsService: ServiceBase, IDealsService
    {
        private const string END_POINT = "deals";
        
        public DealsService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<Deal>> GetDeals(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, END_POINT);
                return await SendRequestForListModels<Deal>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ArrayResult<Deal>.Cancelled() : ArrayResult<Deal>.Error(e.Message);
            }
        }

        public async Task<Result<AssetPurchaseResult>> PurchaseAsset(long assetOfferId)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/{assetOfferId}");
                return await SendPostRequest<AssetPurchaseResult>(url);
            }
            catch (Exception e)
            {
                return Result<AssetPurchaseResult>.Error(e.Message);
            }
        }
        
        public async Task<Result<AssetPurchaseResult>> PurchaseSeasonLevel(int level)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/season-level");
                var body = new
                {
                    TargetLevel = level
                };
                return await SendPostRequest<AssetPurchaseResult>(url, body);
            }
            catch (Exception e)
            {
                return Result<AssetPurchaseResult>.Error(e.Message);
            }
        }
    }
}