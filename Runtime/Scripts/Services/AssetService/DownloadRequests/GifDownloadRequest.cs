using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using UnityEngine;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class GifDownloadRequest: DownloadRequest
    {
        public override Object Asset => null;
        public override byte[] AssetBytes => _assetBytes;
        public override bool IsSuccess => AssetBytes != null;
        private byte[] _assetBytes;

        public override bool AvailableOnlyRawData => true;

        private readonly IRequestHelper _requestHelper;

        public GifDownloadRequest(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public override async Task DownloadAsset(string url, string token, CancellationToken cancellationToken)
        {
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            if (resp.IsSuccess)
            {
                _assetBytes = resp.Data;
            }
            else
            {
                ErrorMessage = resp.DataAsText;
            }
        }
    }
}