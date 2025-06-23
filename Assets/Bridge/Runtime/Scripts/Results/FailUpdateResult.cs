namespace Bridge.Results
{
    internal sealed class FailUpdateResult: FileUpdateResult
    {
        public FailUpdateResult(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}