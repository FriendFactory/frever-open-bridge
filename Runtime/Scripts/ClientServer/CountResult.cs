using Bridge.Results;

namespace Bridge.ClientServer
{
    public class CountResult: Result
    {
        public int Count { get; private set; } = -1;
        
        public static CountResult Success(int count)
        {
            return new CountResult() 
            {
                Count = count
            };
        }

        public static CountResult Cancelled()
        {
            return new CountResult(true);
        }

        public static CountResult Error(string error, int? statusCode = null)
        {
            return new CountResult(error, statusCode);
        }

        private CountResult()
        {
        }

        private CountResult(bool isCanceled): base(isCanceled)
        {
        }
        
        private CountResult(string error, int? statusCode): base(error, statusCode)
        {
        }
    }
}