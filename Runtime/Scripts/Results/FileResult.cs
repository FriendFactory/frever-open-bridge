using Bridge.Services.AssetService;

namespace Bridge.Results
{
    public abstract class FileResult: Result
    {
        public readonly StreamingType? StreamingType;
        public readonly byte[] RawData;

        public bool Cached => FilePath != null;
        public string FilePath { get; protected set; }
        public bool IsStreamedFile => StreamingType.HasValue;
        
        protected FileResult(string filePath, StreamingType? streamingType, byte[] rawData): this(filePath, streamingType)
        {
            RawData = rawData;
        }
        
        protected FileResult(string filePath, StreamingType? streamingType)
        {
            StreamingType = streamingType;
            FilePath = filePath;
        }

        protected FileResult(string errorMessage) : base(errorMessage)
        {
        }

        protected FileResult(bool isCanceled) : base(isCanceled)
        {
        }
    }
}