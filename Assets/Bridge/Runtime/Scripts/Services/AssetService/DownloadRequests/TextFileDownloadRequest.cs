using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using UnityEngine;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class TextFileDownloadRequest: DownloadRequest
    {
        private readonly IRequestHelper _requestHelper;
        private HTTPResponse _response;
        private string _text;
        
        public override Object Asset => new TextAsset(_text);
        public override byte[] AssetBytes => _response.Data;
        public override bool AvailableOnlyRawData => false;
        public override bool IsSuccess => _text != null;

        public TextFileDownloadRequest(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public override async Task DownloadAsset(string url, string token, CancellationToken cancellationToken)
        {
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            _response = await request.GetHTTPResponseAsync(cancellationToken);
            if (_response.IsSuccess)
            {
                _text = _response.DataAsText;
            }
            else
            {
                ErrorMessage = _response.DataAsText;
            }
        }
    }
}