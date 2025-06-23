using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.LevelTests
{
    internal sealed class GetLevelDraftsTest: AuthorizedUserApiTestBase
    {
        public int Top = 20;
        public int Skip = 0;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetLevelDrafts(Top, Skip);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}