namespace Bridge.Results
{
    public sealed class CanceledVideoIdResult: VideoIdResult
    {
        public CanceledVideoIdResult() : base(true)
        {
        }
    }
}