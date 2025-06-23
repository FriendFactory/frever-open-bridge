using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.BodyAnimationTests
{
    internal sealed class GetBodyAnimationTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private long _id;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetBodyAnimationAsync(_id);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}