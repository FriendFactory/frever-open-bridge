using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.BodyAnimationTests
{
    internal sealed class GetRecommendedBodyAnimationsByIdsTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private long _targetId;
        [SerializeField] private int _characterCount = 1;
        [SerializeField] private long _movementType = 1;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetRecommendedBodyAnimationListAsync(_targetId, 10, 10, _movementType, _characterCount, 1);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}