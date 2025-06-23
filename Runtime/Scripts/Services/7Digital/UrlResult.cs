using Bridge.Results;

namespace Bridge.Services._7Digital
{
    public sealed class UrlResult : Result
    {
        public string Url { get; set; }
        public string AuthorizationHeader { get; set;}

        public UrlResult() {}
        
        private UrlResult(bool isCanceled):base(isCanceled) {}
        
        private UrlResult(string errorMessage): base(errorMessage) {}
        
        internal static UrlResult Error(string error)
        {
            return new UrlResult(error);
        }
        
        internal static UrlResult Canceled()
        {
            return new UrlResult(true);
        }
    }
}
