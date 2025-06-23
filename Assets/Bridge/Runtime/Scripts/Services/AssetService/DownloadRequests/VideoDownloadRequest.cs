using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class VideoDownloadRequest: UnityDownloadRequest
    {
        public override Object Asset => null;
        public override bool AvailableOnlyRawData => true;

        protected override UnityWebRequest GetWebRequest(string url)
        {
            return UnityWebRequest.Get(url);
        }
    }
}