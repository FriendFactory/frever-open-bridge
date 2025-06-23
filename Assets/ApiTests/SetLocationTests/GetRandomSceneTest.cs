using UnityEngine;

namespace ApiTests.SetLocationTests
{
    internal sealed class GetRandomSceneTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private int _characterCount = 1;
    
        protected override async void RunTestAsync()
        {
            await Bridge.GetRandomSetLocationSetup(_characterCount);
        }
    }
}