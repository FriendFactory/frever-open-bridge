using System.Linq;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.WardorbeTests
{
    internal sealed class GetWardrobeApiTests : EntityApiTest<Wardrobe>
    {
        public int Take = 10;
        public int TakePrevious = 0;

        protected override async void RunTestAsync()
        {
            var shortInfoList = await Bridge.GetWardrobeList(null,Take, TakePrevious);
            Debug.Log(JsonConvert.SerializeObject(shortInfoList));

            var id = shortInfoList.Models.First().Id;
            var fullInfoResponse = await Bridge.GetWardrobe(id);
            Debug.Log(JsonConvert.SerializeObject(fullInfoResponse));
        }
    }
}
