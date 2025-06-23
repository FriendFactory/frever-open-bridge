using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.DownloadRequests
{
    internal sealed class WavDownloadRequest: UnityDownloadRequest
    {
        public override Object Asset => DownloadHandlerAudioClip.GetContent(Request);
        protected override UnityWebRequest GetWebRequest(string url)
        {
            return UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        }
    }
}