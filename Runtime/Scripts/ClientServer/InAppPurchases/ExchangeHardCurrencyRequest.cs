using Bridge.Models.Common.Files;

namespace Bridge.ClientServer.InAppPurchases
{
    public class ExchangeHardCurrencyRequest
    {
        public long ExchangeProductId { get; set; }

        public Platform Platform { get; set; }
    }
    
    public class ExchangeHardCurrencyResponse: IOkResponse
    {
        public bool Ok { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}