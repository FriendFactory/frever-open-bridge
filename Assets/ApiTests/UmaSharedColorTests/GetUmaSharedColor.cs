using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.UmaSharedColorTests
{
    public class GetUmaSharedColor : EntityApiTest<UmaSharedColor>
    {
        protected override async void RunTestAsync()
        {
            var query = new Query<UmaSharedColor>();
            var resp = await Bridge.GetAsync(query);
            
            Debug.Log(JsonConvert.SerializeObject(resp.Models));
        }
    }
}
