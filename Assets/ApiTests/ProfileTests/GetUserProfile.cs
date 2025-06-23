using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    internal sealed class GetUserProfile: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var res = await Bridge.GetCurrentUserInfo();
            Debug.Log(JsonConvert.SerializeObject(res.Profile));
        }
    }
}