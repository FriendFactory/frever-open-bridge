using Bridge.Results;

namespace Bridge.Services.ContentModeration
{
    public sealed class ModeratedContentResult : Result
    {
        public bool PassedModeration { get; internal set; }
        public string Reason { get; internal set; }

        private ModeratedContentResult()
        {
        }
        
        private ModeratedContentResult(string error):base(error)
        {
        }

        private ModeratedContentResult(bool isCanceled) : base(isCanceled)
        {
        }

        internal static ModeratedContentResult Success(bool passedModeration, string reason)
        {
            return new ModeratedContentResult()
            {
                PassedModeration = passedModeration,
                Reason = reason
            };
        }

        internal static ModeratedContentResult Error(string errorMessage)
        {
            return new ModeratedContentResult(errorMessage);
        }
    }
}
