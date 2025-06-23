using System.Net;

namespace Bridge.VideoServer
{
    public sealed class VideoUploadResult
    {
        public readonly long VideoId;
        public readonly ErrorData ErrorData;
        
        public bool IsSuccess => VideoId != 0;

        internal VideoUploadResult(long videoId)
        {
            VideoId = videoId;
        }

        internal VideoUploadResult(HttpStatusCode errorCode, string errorMessage)
        {
            ErrorData = new ErrorData(errorCode, errorMessage);
        }
        
        internal VideoUploadResult(ErrorData errorData)
        {
            ErrorData = errorData;
        }
    }
}