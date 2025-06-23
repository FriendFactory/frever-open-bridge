using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    public class StartFollowTest: AuthorizedUserApiTestBase 
    {
        protected override async void RunTestAsync()
        {
            var allGroupsRes = await Bridge.GetProfiles(10, 0);
            var resp = await Bridge.StartFollow(allGroupsRes.Profiles.Last().MainGroupId);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}
