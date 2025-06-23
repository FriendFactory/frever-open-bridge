using UnityEngine;

namespace ApiTests.GroupTests
{
    internal sealed class GetGroupShortInfosTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private long[] _ids;
        
        protected override async void RunTestAsync()
        {
            await Bridge.GetProfilesShortInfo(_ids);
        }
    }
}