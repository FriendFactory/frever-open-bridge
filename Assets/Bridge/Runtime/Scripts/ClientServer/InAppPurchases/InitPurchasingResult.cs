using System;
using Bridge.Results;

namespace Bridge.ClientServer.InAppPurchases
{
    public sealed class InitPurchasingResult: Result
    {
        public Guid PendingOrderId { get; private set; }
        
        internal static InitPurchasingResult Success(Guid pendingOrderId)
        {
            return new InitPurchasingResult
            {
                PendingOrderId = pendingOrderId
            };
        }
        
        internal static InitPurchasingResult Error(string errorMessage)
        {
            return new InitPurchasingResult(errorMessage);
        }

        private InitPurchasingResult(): base()
        {
        }
        
        private InitPurchasingResult(string error): base(error)
        {
        }
    }
}