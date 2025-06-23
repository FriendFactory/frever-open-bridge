using System.Net;

namespace Bridge.VideoServer
{
    public class ErrorData
    {
        public readonly HttpStatusCode StatusCode;
        public readonly string ErrorMessage;

        internal ErrorData(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}