using Bridge.Results;

namespace Bridge.Services.AssetService
{
    internal sealed class ConvertingUrlResponse: Result
    {
        public readonly string SourceFileDestinationUrl;
        public readonly string CheckingFileExistenceUrl;
        public readonly string ConvertedFileUrl;
        public readonly string ConvertedFileExtension;
        public readonly string UploadId;

        public ConvertingUrlResponse(string sourceFileDestinationUrl, string checkingFileExistenceUrl,
            string convertedFileUrl, string convertedFileExtension, string uploadId)
        {
            CheckingFileExistenceUrl = checkingFileExistenceUrl;
            ConvertedFileUrl = convertedFileUrl;
            ConvertedFileExtension = convertedFileExtension;
            SourceFileDestinationUrl = sourceFileDestinationUrl;
            UploadId = uploadId;
        }

        public ConvertingUrlResponse(string error): base(error)
        {
        }
    }
}