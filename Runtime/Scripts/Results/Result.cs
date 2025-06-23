namespace Bridge.Results
{
    public class Result<T>: Result
    {
        public readonly T Model;

        private static Result<T> _cachedCancelledRequest;
        
        internal static Result<T> Success(T model)
        {
            return new Result<T>(model);
        }

        internal static Result<T> Error(string error, int? statusCode = null)
        {
            return new Result<T>(error, statusCode);
        }
        
        internal static Result<T> Cancelled()
        {
            return _cachedCancelledRequest ?? (_cachedCancelledRequest = new Result<T>(true));
        }
        
        private Result(T model)
        {
            Model = model;
        }
        
        private Result(bool isCanceled): base(isCanceled)
        {
        }

        private Result(string error, int? statusCode) : base(error, statusCode)
        {
        }

    }
    
    public abstract class Result
    {
        public bool IsError { get; protected set; }
        public string ErrorMessage  { get; protected set; }
        public bool IsRequestCanceled  { get; protected set; }
        public int? HttpStatusCode  { get; protected set; }

        public bool IsSuccess => !IsError && !IsRequestCanceled;
        
        protected Result(bool isRequestCanceled = false)
        {
            IsRequestCanceled = isRequestCanceled;
        }

        protected Result(string errorMessage, int? httpStatusCode = null)
        {
            HttpStatusCode = httpStatusCode;
            ErrorMessage = errorMessage;
            IsError = true;
        }
    }
}