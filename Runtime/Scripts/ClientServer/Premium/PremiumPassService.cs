using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Premium
{
    internal interface IPremiumPassService
    {
        Task<Result> BuyPremium();
    }

    internal sealed class PremiumPassService : ServiceBase, IPremiumPassService
    {
        public PremiumPassService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<Result> BuyPremium()
        {
            var url = ConcatUrl(Host, $"gamification/premium");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText);
            }

            var responseModel = Serializer.DeserializeJson<BuyPremiumPassResult>(resp.DataAsText);
            if (responseModel.Ok)
            {
                return new SuccessResult();
            }

            return new ErrorResult($"{responseModel.ErrorCode}. {responseModel.ErrorDescription}");
        }
        
        private sealed class BuyPremiumPassResult
        {
            public bool Ok { get; set; }

            public string ErrorCode { get; set; }

            public string ErrorDescription { get; set; }
        }
    }
}