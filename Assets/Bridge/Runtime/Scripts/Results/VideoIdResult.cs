namespace Bridge.Results
{
    public class VideoIdResult: Result
    {
        public readonly long VideoId;

        internal VideoIdResult(long videoId)
        {
            VideoId = videoId;
        }

        internal VideoIdResult(string error):base(error)
        {
        }

        protected VideoIdResult(bool isCanceled) : base(isCanceled)
        {
        }
    }
}