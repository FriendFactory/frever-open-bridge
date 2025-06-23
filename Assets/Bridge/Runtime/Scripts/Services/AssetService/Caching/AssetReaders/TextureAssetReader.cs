using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class TextureAssetReader : UnityAssetReader
    {
        protected override string[] PossibleExtensions => new[] { ".jpg", ".jpeg", ".png" };

        protected override UnityWebRequest CreateRequest(string path)
        {
            return UnityWebRequestTexture.GetTexture(path, true);
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request)
        {
            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
