namespace Bridge.Results
{
    public sealed class CanceledResult: Result
    {
        internal CanceledResult() : base(true)
        {
        }
    }
}