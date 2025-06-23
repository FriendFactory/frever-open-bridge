using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge.Services.AssetService
{
    internal class FileUploadResult: Result
    {
        public string UploadId;
        public FileInfo FileInfo;

        public FileUploadResult(FileInfo fileInfo, string uploadId)
        {
            FileInfo = fileInfo;
            UploadId = uploadId;
        }

        public FileUploadResult(string errorMessage) : base(errorMessage)
        {
        }
    }
}