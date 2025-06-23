using UnityEngine;

namespace ApiTests.DownloadingAssetTests
{
    internal sealed class DownloadImageByKey: AuthorizedUserApiTestBase
    {
        [SerializeField] private string _key;
        [SerializeField] private bool _cache;
        
        protected override async void RunTestAsync()
        {
            var imageResp = await Bridge.GetImageAsync(_key, _cache);
        }
    }
}