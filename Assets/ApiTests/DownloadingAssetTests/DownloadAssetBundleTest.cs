    using System.Linq;
using Bridge.ExternalPackages.AsynAwaitUtility;
    using UnityEngine;
using UnityEngine.SceneManagement;

namespace ApiTests.DownloadingAssetTests
{
    internal sealed class DownloadAssetBundleTest : AuthorizedUserApiTestBase
    {
        public long SetLocationId;
        public bool CacheFile;
        protected override async void RunTestAsync()
        {
            var response = await Bridge.GetSetLocationAsync(SetLocationId);
            var setLocation = response.Model;

            var assetResult = await Bridge.GetAssetAsync(setLocation.SetLocationBundle, CacheFile);
            Debug.Log("Loaded success: " + assetResult.Object);
            var scenePath = (assetResult.Object as AssetBundle).GetAllScenePaths().First();
            await SceneManager.LoadSceneAsync(scenePath);
        }
    }
}
