using UnityEngine;

namespace ApiTests.LevelTests
{
    public sealed class GetShuffledLevelApiTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private long _id;
        
        protected override async void RunTestAsync()
        {
            await Bridge.GetShuffledLevel(_id);
        }
    }
}