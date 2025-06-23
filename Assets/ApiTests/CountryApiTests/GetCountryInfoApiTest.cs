using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.CountryApiTests
{
    internal sealed class GetCountryInfoApiTest : AnonymousAccessedEndPointTest
    {
        [SerializeField] private string _isoCode = "SWE";
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetCountryInfoAsync(_isoCode);
            if (resp.IsError)
            {
                Debug.LogError(resp.ErrorMessage);
                return;
            }
            Debug.Log(JsonConvert.SerializeObject(resp.Model));
        }
    }
}
