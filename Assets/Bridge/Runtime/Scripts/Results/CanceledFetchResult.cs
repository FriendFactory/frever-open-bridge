namespace Bridge.Results
{
    public sealed class CanceledFetchResult: FetchResult
    {
        internal CanceledFetchResult() : base(true)
        {
        }
    }
}