using Bridge.Models.Common.Files;

namespace Bridge.ClientServer.InAppPurchases
{
    public class BuyPremiumPassRequest
    {
        public Platform Platform { get; set; }

        public string Receipt { get; set; }
    }
    
    public class BuyPremiumPassResponse: IOkResponse
    {
        public bool Ok { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}