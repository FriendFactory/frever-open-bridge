using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal abstract class UnityDownloadRequest: DownloadRequest
    {
        private const int RETRY_COUNT = 3;
        
        public sealed override byte[] AssetBytes => Request.downloadHandler.data;
        public override bool AvailableOnlyRawData => false;
        protected UnityWebRequest Request { get; private set; }

        public sealed override async Task DownloadAsset(string url, string token, CancellationToken cancellationToken)
        {
            var attempt = 0;
            do
            {
                await DownloadAssetInternal(url, token, cancellationToken);
                attempt++;
            } while (attempt < RETRY_COUNT && !IsSuccess);
        }

        private async Task DownloadAssetInternal(string url, string accessToken, CancellationToken cancellationToken)
        {
            Request = GetWebRequest(url);
            Request.SetRequestHeader("Authorization", "Bearer " + accessToken);
                
            var asyncOperation = Request.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Delay(25, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            IsSuccess = Request.result is not (UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError);
            if (!IsSuccess)
            {
                ErrorMessage = Request.error;
            }
        }

        protected abstract UnityWebRequest GetWebRequest(string url);

        public override void Dispose()
        {
            Request?.Dispose();
        }
    }
}