namespace Bridge.Results
{
    public class ArrayResult<T>: Result
    {
        public readonly T[] Models;

        private static ArrayResult<T> _cachedCancelledResult;

        internal ArrayResult(T[] models)
        {
            Models = models;
        }

        internal ArrayResult(string errorMessage, int? statusCode = null) : base(errorMessage, statusCode)
        {
        }

        protected ArrayResult(bool isCanceled) : base(isCanceled)
        {
        }

        internal static ArrayResult<T> Success(T[] models)
        {
            return new ArrayResult<T>(models);
        }

        internal static ArrayResult<T> Error(string error)
        {
            return new ArrayResult<T>(error);
        }

        internal static ArrayResult<T> Cancelled()
        {
            return _cachedCancelledResult ?? (_cachedCancelledResult = new ArrayResult<T>(true));
        }
    }
}