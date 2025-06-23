using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.Common.Files;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.Services.AssetService
{
    internal sealed class FilesFromDiskUploader: FileUploader
    {
        public override FilePlacingType TargetFilePlacingType => FilePlacingType.OnDisk;

        public FilesFromDiskUploader(string serverUrl, IRequestHelper requestHelper) : base(serverUrl, requestHelper)
        {
        }

        protected override bool Validate(FileInfo fileInfo, out string failReason)
        {
            var pathNotNull = !string.IsNullOrEmpty(fileInfo.FilePath);
            if (!pathNotNull)
            {
                failReason = $"Path is null for {fileInfo.FileType}";
                return false;
            }

            var fileExists = fileInfo.Exists();
            if (!fileExists)
            {
                failReason = $"File does not exist locally {fileInfo.FilePath}";
                return false;
            }

            failReason = null;
            return true;
        }

        protected override async Task<HTTPResponse> UploadFileAsync(HTTPRequest request, FileInfo fileInfo, CancellationToken cancellationToken)
        {
            var stream = File.OpenRead(fileInfo.FilePath);
            request.AddHeader("Content-Type", "application/octet-stream");
            request.UploadStream = stream;
            request.DisposeUploadStream = true;
            return await request.GetHTTPResponseAsync(cancellationToken);
        }
    }
}