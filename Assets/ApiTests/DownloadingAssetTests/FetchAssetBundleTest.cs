using UnityEngine;

namespace ApiTests.DownloadingAssetTests
{
    internal sealed class FetchAssetBundleTest : AuthorizedUserApiTestBase
    {
        public long SetLocationId;
      
        protected override async void RunTestAsync()
        {
            await Bridge.ClearCacheAsync();
            
            var response = await Bridge.GetSetLocationAsync(SetLocationId);
            var setLocation = response.Model;

            var e = await Bridge.FetchMainAssetAsync(setLocation.SetLocationBundle);
            Debug.Log("Fetched success: " + e.IsSuccess);
        }
    }
}