namespace Bridge.Services.TranscodingService
{
    public class ConvertedFileCacheEntry
    {
        public readonly string FilePath;
        public readonly string UploadId;

        public ConvertedFileCacheEntry(string fileDestPath, string uploadId)
        {
            FilePath = fileDestPath;
            UploadId = uploadId;
        }
    }
}