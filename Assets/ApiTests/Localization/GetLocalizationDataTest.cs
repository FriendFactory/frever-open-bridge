using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.Localization
{
    internal sealed class GetLocalizationDataTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private string _isoCode = "US";
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetLocalizationData(_isoCode);
            Debug.Log($"{JsonConvert.SerializeObject(resp)}");
        }
    }
}
