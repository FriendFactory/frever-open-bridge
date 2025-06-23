using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using UnityEngine;

namespace ApiTests.DownloadingAssetTests
{
    public class MultipleFilesDownloadingTests : EntityApiTest<UmaBundle>
    {
        protected override async void RunTestAsync()
        {
            var cacheCleaningResult = await Bridge.ClearCacheAsync();
            if (cacheCleaningResult.IsError)
            {
                Debug.LogError(cacheCleaningResult.ErrorMessage);
                return;
            }
            var bigBundleRequest = new Query<UmaBundle>();
            bigBundleRequest.SetFilters(new FilterSetup()
            {
               FieldName = nameof(UmaBundle.UmaBundleTypeId),
               FilterType = FilterType.Equals,
               FilterValue = 3 
            });
            bigBundleRequest.SetMaxTop(1);

            var resp = await Bridge.GetAsync(bigBundleRequest);
            var umaBundle = resp.Models.First();

#pragma warning disable CS4014
            Bridge.GetAssetAsync(umaBundle);
            Bridge.GetAssetAsync(umaBundle);
            Bridge.GetAssetAsync(umaBundle);
            Bridge.GetAssetAsync(umaBundle);
            Bridge.GetAssetAsync(umaBundle);
#pragma warning restore CS4014
        }
    }
}
