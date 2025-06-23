using Bridge.Models.AsseManager;
using Bridge.Models.ClientServer.Assets;
using Bridge.Results;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.SetLocationTests
{
    internal sealed class GetSetLocationListTest : EntityApiTest<SetLocation>
    {
        public int Take = 10;
        public int Previous = 0;
        public int Category = 0;
        public long Race = 1;
        public string Filter;
    
        protected override async void RunTestAsync()
        {
            ArrayResult<SetLocationFullInfo> resp;
            if (Category != 0)
            {
                resp = await Bridge.GetSetLocationListAsync(null,Take, Previous, Race, Category, Filter);
            }
            else
            {
                resp = await Bridge.GetSetLocationListAsync(null,Take, Previous, Race, filter: Filter);
            }
        
            Debug.Log(JsonConvert.SerializeObject(resp.Models));
        }
    }
}