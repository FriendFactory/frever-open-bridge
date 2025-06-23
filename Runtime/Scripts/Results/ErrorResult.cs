namespace Bridge.Results
{
    public sealed class ErrorResult: Result
    {
        public ErrorResult(string errorMessage, int? statusCode = null) : base(errorMessage, statusCode)
        {
        }
    }
}