using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.LevelTests
{
    internal sealed class GetLevelThumbnailDataTest: AuthorizedUserApiTestBase
    {
        public long LevelId;
        
        protected override async void RunTestAsync()
        {
            var req = await Bridge.GetLevelThumbnailInfo(LevelId);
            Debug.Log($"{JsonConvert.SerializeObject(req)}");
        }
    }
}