using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.WardrobeTests
{
    internal sealed class InvalidateWardrobeApiTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private long _wardrobeId;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.InvalidateWardrobe(_wardrobeId, "Bridge test");
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}