using Bridge.AssetManagerServer;
using Bridge.Models.Common.Files;

namespace Bridge.Results
{
    public sealed class ConvertingResult: Result
    { 
        public string ConvertedFilePath { get; private set; }
        public string UploadId { get; private set; }
        public FileExtension Extension { get; private set; }

        private ConvertingResult(bool canceled):base(canceled)
        {
        }
        
        private ConvertingResult(string error):base(error)
        {
        }
        
        public static ConvertingResult GetSuccessResult(string pathToResultFile, string uploadId)
        {
            return new ConvertingResult(false)
            {
                ConvertedFilePath = pathToResultFile,
                UploadId = uploadId,
                Extension = FileExtensionManager.GetFileExtension(pathToResultFile)
            };
        }
        
        public static ConvertingResult GetFailedResult(string errorMessage)
        {
            return new ConvertingResult(errorMessage);
        }
    }
}