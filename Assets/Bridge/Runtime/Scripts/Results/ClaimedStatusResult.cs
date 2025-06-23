namespace Bridge.Results
{
    public sealed class ClaimedStatusResult: Result
    {
        public readonly bool IsClaimed;

        private ClaimedStatusResult(bool isClaimed)
        {
            IsClaimed = isClaimed;
        }

        private ClaimedStatusResult(string errorMessage, int? statusCode):base(errorMessage, statusCode)
        {
        }
        
        private ClaimedStatusResult():base(true)
        {
            
        }
        
        public static ClaimedStatusResult Success(bool isClaimed)
        {
            return new ClaimedStatusResult(isClaimed);
        }
        
        public static ClaimedStatusResult Error(string message, int? statusCode)
        {
            return new ClaimedStatusResult(message, statusCode);
        }

        public static ClaimedStatusResult Cancelled()
        {
            return new ClaimedStatusResult();
        }
    }
}