using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class Mp3DownloadRequest: UnityDownloadRequest
    {
        public override Object Asset => DownloadHandlerAudioClip.GetContent(Request);
        protected override UnityWebRequest GetWebRequest(string url)
        {
            var wr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            ((DownloadHandlerAudioClip)wr.downloadHandler).streamAudio = false;
            return wr;
        }
    }
}