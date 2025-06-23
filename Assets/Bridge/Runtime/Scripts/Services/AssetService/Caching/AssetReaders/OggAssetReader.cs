using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class OggAssetReader : UnityAssetReader
    {
        protected override string[] PossibleExtensions => new[] { ".ogg" };

        protected override UnityWebRequest CreateRequest(string path)
        {
            var wr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS);
            ((DownloadHandlerAudioClip)wr.downloadHandler).streamAudio = false;
            return wr;
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request)
        {
            return DownloadHandlerAudioClip.GetContent(request);
        }
    }
}
