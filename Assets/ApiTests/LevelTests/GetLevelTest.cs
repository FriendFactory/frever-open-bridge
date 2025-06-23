using ApiTests.Levels;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.LevelTests
{
    public class GetLevelTest : LevelEntityApiTest
    {
        protected override async void RunTestAsync()
        {
            var anyLevel = await GetAnyAvailableEntityId<Level>();
            var fullDataResp = await Bridge.GetAsync<Level>(anyLevel);
            Debug.Log(JsonConvert.SerializeObject(fullDataResp.ResultObject));
        }
    }
}
