using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.StartPackFetchTests
{
    public class MetadataStartPackTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var req = await Bridge.GetMetadataStartPackAsync();
            if (req.IsError)
            {
                Debug.LogError(req.ErrorMessage);
            }
            else
            {
                Debug.Log($"{JsonConvert.SerializeObject(req.Pack)}");
            }
        }
    }
}