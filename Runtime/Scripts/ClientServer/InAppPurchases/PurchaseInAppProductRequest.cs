using System;
using Bridge.Models.Common.Files;

namespace Bridge.ClientServer.InAppPurchases
{
    internal class InitInAppPurchaseRequest
    {
        /// <summary>
        ///     Use encoded string contains information about assets and other benefits offered to the user.
        /// </summary>
        public string InAppProductOfferKey { get; set; }
        
        public string ClientCurrency { get; set; }

        public decimal ClientCurrencyPrice { get; set; }
    }

    public class InitInAppPurchaseResponse: IOkResponse
    {
        public Guid PendingOrderId { get; set; }

        public bool Ok { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class CompleteInAppPurchaseRequest
    {
        public Guid PendingOrderId { get; set; }

        public Platform Platform { get; set; }

        public string Receipt { get; set; }
    }

    public class CompleteInAppPurchaseResponse: IOkResponse
    {
        public bool Ok { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}