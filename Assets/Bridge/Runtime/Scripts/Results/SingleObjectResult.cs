namespace Bridge.Results
{
    public class SingleObjectResult<T>: Result
    {
        public readonly T ResultObject;

        internal SingleObjectResult(T resultObject)
        {
            ResultObject = resultObject;
        }

        internal SingleObjectResult(string errorMessage) : base(errorMessage)
        {
        }

        protected SingleObjectResult(bool isCanceled) : base(isCanceled)
        {
        }
    }
}