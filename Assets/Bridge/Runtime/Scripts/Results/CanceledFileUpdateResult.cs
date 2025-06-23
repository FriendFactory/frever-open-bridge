namespace Bridge.Results
{
    public sealed class CanceledFileUpdateResult: FileUpdateResult
    {
        public CanceledFileUpdateResult() : base(true)
        {
        }
    }
}