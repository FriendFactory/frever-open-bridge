using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class Mp3AssetReader : UnityAssetReader
    {
        protected override string[] PossibleExtensions => new[] { ".mp3" };

        protected override UnityWebRequest CreateRequest(string path)
        {
            var wr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
            ((DownloadHandlerAudioClip)wr.downloadHandler).streamAudio = false;
            return wr;
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request)
        {
            return DownloadHandlerAudioClip.GetContent(request);
        }
    }
}
