using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using JetBrains.Annotations;

namespace Bridge.ClientServer.InAppPurchases
{
    internal interface IInAppPurchasesService
    {
        Task<Result<AvailableOffers>> GetProductOffers(CancellationToken token);
        Task<InitPurchasingResult> InitPurchasingInAppProduct(string productOfferKey, string currency, decimal price);
        Task<Result> CompletePurchasingInAppProduct(Guid pendingOrderId, string receipt, Platform platform);
        Task<Result> ExchangeHardCurrency(long exchangeProductId, Platform platform);
    }

    [UsedImplicitly]
    internal sealed class InAppPurchasesService: ServiceBase, IInAppPurchasesService
    {
        private const string END_POINT = "in-app-purchase";
        public InAppPurchasesService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public Task<Result<AvailableOffers>> GetProductOffers(CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/offers");
            return SendRequestForSingleModel<AvailableOffers>(url, token);
        }

        public async Task<InitPurchasingResult> InitPurchasingInAppProduct(string productOfferKey, string currency, decimal price)
        {
            var url = BuildUrl("init");
            var body = new InitInAppPurchaseRequest
            {
                InAppProductOfferKey = productOfferKey,
                ClientCurrency = currency,
                ClientCurrencyPrice = price
            };
            var resp = await SendPostRequest<InitInAppPurchaseResponse>(url, body);
            if (resp.IsSuccess)
            {
                return resp.Model.PendingOrderId == default 
                    ? InitPurchasingResult.Error($"Wrong order id: {resp.Model.PendingOrderId}") 
                    : InitPurchasingResult.Success(resp.Model.PendingOrderId);
            }
            return InitPurchasingResult.Error(resp.ErrorMessage);
        }

        public async Task<Result> CompletePurchasingInAppProduct(Guid pendingOrderId, string receipt, Platform platform)
        {
            var url =  BuildUrl("complete");
            var body = new CompleteInAppPurchaseRequest
            {
                PendingOrderId = pendingOrderId,
                Receipt = receipt,
                Platform = platform
            };
            return await SendPostRequest<CompleteInAppPurchaseResponse>(url, body);
        }

        public async Task<Result> ExchangeHardCurrency(long exchangeProductId, Platform platform)
        {
            var url = BuildUrl("exchange-hard-currency");
            var body = new ExchangeHardCurrencyRequest
            {
                ExchangeProductId = exchangeProductId,
                Platform = platform
            };
            return await SendPostRequest<ExchangeHardCurrencyResponse>(url, body);
        }

        private string BuildUrl(string endPoint)
        {
            return ConcatUrl(Host, $"{END_POINT}/{endPoint}");
        }
    }
}