using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    public class GetAllFollowersTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var getAllFollowers = await Bridge.GetFollowedByCurrentUser(20, 0);
            if (getAllFollowers.IsError)
            {
                Debug.LogError(getAllFollowers.ErrorMessage);
                return;
            }
            
            Debug.Log(JsonConvert.SerializeObject(getAllFollowers.Profiles));
        }
    }
}