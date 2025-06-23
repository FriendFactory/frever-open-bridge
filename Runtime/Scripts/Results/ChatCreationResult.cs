namespace Bridge.Results
{
    public sealed class ChatCreationResult: Result
    {
        public long CreatedChatId { get; private set; }

        private ChatCreationResult(bool isCancelled = false): base(isCancelled)
        {
        }

        private ChatCreationResult(string error, int? httpStatusCode): base(error, httpStatusCode)
        {
        }
        
        internal static ChatCreationResult Success(long createdChatId)
        {
            return new ChatCreationResult
            {
                CreatedChatId = createdChatId,
            };
        }

        internal static ChatCreationResult Cancelled()
        {
            return new ChatCreationResult(true);
        }

        internal static ChatCreationResult Error(string error, int? httpStatusCode = null)
        {
            return new ChatCreationResult(error, httpStatusCode);
        }

        internal static ChatCreationResult Result(Result<long> baseResult)
        {
            if (baseResult.IsError) return Error(baseResult.ErrorMessage, baseResult.HttpStatusCode);
            if (baseResult.IsRequestCanceled) return Cancelled();
            return Success(baseResult.Model);
        }
    }
}