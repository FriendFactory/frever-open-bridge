using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class ImageDownloadRequest: DownloadRequest
    {
        private readonly IRequestHelper _requestHelper;
        private Texture2D _texture;
        private HTTPResponse _response;
        
        public override bool IsSuccess => _texture != null;
        public override Object Asset => _texture;
        public override byte[] AssetBytes => _response.Data;
        public override bool AvailableOnlyRawData => false;

        public ImageDownloadRequest(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public override async Task DownloadAsset(string url, string token, CancellationToken cancellationToken)
        {
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            _response = await request.GetHTTPResponseAsync(cancellationToken);
            if (_response.IsSuccess)
            {
                _texture = _response.DataAsTexture2D;
            }
            else
            {
                ErrorMessage = _response.DataAsText;
            }
        }
    }
}