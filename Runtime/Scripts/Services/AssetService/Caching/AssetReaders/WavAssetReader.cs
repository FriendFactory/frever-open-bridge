using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class WavAssetReader : UnityAssetReader
    {
        protected override string[] PossibleExtensions => new[] { ".wav" };

        protected override UnityWebRequest CreateRequest(string path)
        {
            return UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request)
        {
            return DownloadHandlerAudioClip.GetContent(request);
        }
    }
}
