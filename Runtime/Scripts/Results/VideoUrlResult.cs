namespace Bridge.Results
{
    public sealed class VideoUrlResult: Result
    {
        public string Url { get; private set; }
        
        private VideoUrlResult(bool isCanceled):base(isCanceled)
        {
        }

        private VideoUrlResult(string error): base(error)
        {
        }
        
        internal static VideoUrlResult Success(string url)
        {
            var resp = new VideoUrlResult(false) {Url = url};
            return resp;
        }
        
        internal static VideoUrlResult Canceled()
        {
            return new VideoUrlResult(true);
        }

        internal static VideoUrlResult Error(string errorMessage)
        {
            return new VideoUrlResult(errorMessage);
        }
    }
}