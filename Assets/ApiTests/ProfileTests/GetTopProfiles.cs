using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    public class GetTopProfiles : AuthorizedUserApiTestBase
    {
        public int Skip;
        public int Take = 20;
        protected override async void RunTestAsync()
        {
            var response = await Bridge.GetProfiles(Take, Skip);
            if (response.IsSuccess)
            {
                Debug.Log(JsonConvert.SerializeObject(response.Profiles));
            }
            else
            {
                Debug.LogError(response.ErrorMessage);
            }
        }
    }
}
