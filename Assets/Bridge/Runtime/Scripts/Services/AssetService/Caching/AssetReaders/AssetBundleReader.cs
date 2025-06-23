using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class AssetBundleReader : UnityAssetReader
    {
        protected override string[] PossibleExtensions => new[] { string.Empty };

        protected override UnityWebRequest CreateRequest(string path)
        {
            return UnityWebRequestAssetBundle.GetAssetBundle(path);
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request)
        {
            var bundle = DownloadHandlerAssetBundle.GetContent(request);
            return bundle;
        }

        protected override void OnAssetExtracted(Object asset, CancellationToken token)
        {
            base.OnAssetExtracted(asset, token);
            //unload asset bundle if cancellation requested while AssetLoadingJob is not finished
            //it will not happen after bridge returns the bundle, because the token is created in AssetLoadingJob
            //and being triggered only if job is not finished
            token.Register(() => (asset as AssetBundle)?.Unload(true));
        }
    }
}
