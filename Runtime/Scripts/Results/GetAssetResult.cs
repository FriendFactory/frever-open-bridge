using Bridge.Services.AssetService;

namespace Bridge.Results
{
    public class GetAssetResult: FileResult
    {
        public readonly object Object;

        internal GetAssetResult(object o, string filePath, StreamingType? streamingType = null,  byte[] rawData = null): base(filePath, streamingType, rawData)
        {
            Object = o;
            FilePath = filePath;
        }

        internal GetAssetResult(string errorMessage) : base(errorMessage)
        {
        }

        protected GetAssetResult(bool isCanceled) : base(isCanceled)
        {
        }
    }
}