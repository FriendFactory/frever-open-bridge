using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.BodyAnimationTests
{
    internal sealed class GetBodyAnimationsByIdsTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private long[] _ids;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetBodyAnimationByIdsAsync(_ids);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}