using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.RandomAssets
{
    public class GetRandomAssetsTest : AuthorizedUserApiTestBase
    {
        public int CharacterCount = 1;
    
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetRandomSetLocationSetup(CharacterCount);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}
