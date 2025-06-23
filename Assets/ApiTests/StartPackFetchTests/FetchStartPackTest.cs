using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.StartPackFetchTests
{
    public class FetchStartPackTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var req = await Bridge.FetchStartPackAssets(5);
            if (req.IsError)
            {
                Debug.LogError(req.ErrorMessage);
            }
            else
            {
                Debug.Log($"{JsonConvert.SerializeObject(req.IsSuccess)}");
            }
        }
    }
}
