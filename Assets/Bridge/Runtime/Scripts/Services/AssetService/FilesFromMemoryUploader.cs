using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService
{
    internal sealed class FilesFromMemoryUploader: FileUploader
    {
        public FilesFromMemoryUploader(string serverUrl, IRequestHelper requestHelper) : base(serverUrl, requestHelper)
        {
        }

        public override FilePlacingType TargetFilePlacingType => FilePlacingType.InMemory;

        protected override bool Validate(FileInfo fileInfo, out string failReason)
        {
            var hasRawData = fileInfo.FileRawData != null;
            if (!hasRawData)
            {
                failReason = $"File info does not contain raw data (byte[])";
                return false;
            }

            failReason = null;
            return true;
        }

        protected override async Task<HTTPResponse> UploadFileAsync(HTTPRequest request, FileInfo fileInfo, CancellationToken cancellationToken)
        {
            request.RawData = fileInfo.FileRawData;
            return await request.GetHTTPResponseAsync(cancellationToken);
        }
    }
}