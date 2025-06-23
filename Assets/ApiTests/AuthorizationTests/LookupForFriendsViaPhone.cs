using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class LookupForFriendsViaPhone:AuthorizedUserApiTestBase
    {
        public string[] PhoneNumbers;
        
        protected override async void RunTestAsync()
        {
            var result = await Bridge.LookupForFriends(PhoneNumbers);
            Debug.Log(JsonConvert.SerializeObject(result));
        }
    }
}