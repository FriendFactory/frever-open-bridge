using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class OggDownloadRequest: UnityDownloadRequest
    {
        public override Object Asset => DownloadHandlerAudioClip.GetContent(Request);
        protected override UnityWebRequest GetWebRequest(string url)
        {
            var wr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS);
            ((DownloadHandlerAudioClip)wr.downloadHandler).streamAudio = false;
            return wr;
        }
    }
}