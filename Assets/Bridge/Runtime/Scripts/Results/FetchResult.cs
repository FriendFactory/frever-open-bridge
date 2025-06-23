using Bridge.Services.AssetService;

namespace Bridge.Results
{
    public class FetchResult: FileResult
    {
        internal FetchResult(string filePath, StreamingType? streamingType = null):base(filePath, streamingType)
        {
        }

        internal FetchResult(string errorMessage) : base(errorMessage)
        {
        }

        protected FetchResult(bool isCanceled) : base(isCanceled)
        {
        }
    }
}