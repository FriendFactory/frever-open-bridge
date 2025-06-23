namespace Bridge.Results
{
    public sealed class CanceledSingleObjectResult<T>: SingleObjectResult<T>
    {
        internal CanceledSingleObjectResult() : base(true)
        {
        }
    }
}