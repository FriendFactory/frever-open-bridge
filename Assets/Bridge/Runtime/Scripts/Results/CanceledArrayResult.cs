namespace Bridge.Results
{
    public sealed class CanceledArrayResult<T>: ArrayResult<T>
    {
        internal CanceledArrayResult() : base(true)
        {
        }
    }
}