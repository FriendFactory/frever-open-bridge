using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;

namespace Bridge.Services.AssetService
{
    internal abstract class FileUploader
    {
        private readonly IRequestHelper _requestHelper;
        private readonly string _serverUrl;

        protected FileUploader(string serverUrl, IRequestHelper requestHelper)
        {
            _serverUrl = serverUrl;
            _requestHelper = requestHelper;
        }

        public abstract FilePlacingType TargetFilePlacingType { get; }

        public async Task<FileUploadResult> UploadAsync(FileInfo fileInfo, CancellationToken cancellationToken)
        {
            if (!Validate(fileInfo, out var validationFailReason)) return new FileUploadResult(validationFailReason);
            
            var initResp = await InitUploading(cancellationToken);

            if (initResp.IsError)
                return new FileUploadResult(initResp.ErrorMessage);

            var url = initResp.UploadUrl;
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Put, false, false);
            var resp = await UploadFileAsync(request, fileInfo, cancellationToken);
            if (!resp.IsSuccess)
                return new FileUploadResult(resp.DataAsText);
                
            fileInfo.Source.UploadId = initResp.UploadId;
            return new FileUploadResult(fileInfo, initResp.UploadId);
        }

        protected abstract bool Validate(FileInfo fileInfo, out string failReason);

        protected abstract Task<HTTPResponse> UploadFileAsync(HTTPRequest request, FileInfo fileInfo,
            CancellationToken cancellationToken);

        private async Task<InitUploadResp> InitUploading(CancellationToken cancellationToken)
        {
            var url = $"{_serverUrl}File/PreUploadingUrl";
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var response = await request.GetHTTPResponseAsync(cancellationToken);
            if (!response.IsSuccess)
                return new InitUploadResp(response.DataAsText);

            var respJson = response.DataAsText;
            var respModel = JsonConvert.DeserializeObject<InitUploadResp>(respJson);
            return respModel;
        }
    }
}