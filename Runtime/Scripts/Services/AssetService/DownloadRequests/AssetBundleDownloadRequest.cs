using System;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class AssetBundleDownloadRequest : DownloadRequest
    {
        private readonly IRequestHelper _requestHelper;
        private AssetBundle _assetBundle;

        private byte[] _assetBundleBytes;

        public AssetBundleDownloadRequest(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public override Object Asset
        {
            get
            {
                if (_assetBundle == null) _assetBundle = AssetBundle.LoadFromMemory(_assetBundleBytes);
                return _assetBundle;
            }
        }

        public override byte[] AssetBytes => _assetBundleBytes;
        public override bool IsSuccess => _assetBundleBytes != null;
        public override bool AvailableOnlyRawData => false;

        public override Task DownloadAsset(string url, string token, CancellationToken cancellationToken)
        {
            return DownloadAsync(url, cancellationToken);
        }

        private async Task DownloadAsync(string url, CancellationToken cancellationToken)
        {
            //has to download via not UnityWebRequest, because we can't get RawData from AssetBundle UnityWebRequest
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            //todo: hot fix for TaskCanceledException thrown by http client. Cards: frev-4930, 4934
            try
            {
                var resp = await req.GetHTTPResponseAsync(cancellationToken);
                if (!resp.IsSuccess)
                {
                    ErrorMessage = resp.DataAsText;
                    return;
                }

                _assetBundleBytes = resp.Data;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        public override void Dispose()
        {
        }
    }
}