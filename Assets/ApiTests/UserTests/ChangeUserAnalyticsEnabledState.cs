using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.UserTests
{
    public class ChangeUserAnalyticsEnabledState : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var currentUser = (await Bridge.GetAsync<User>(Bridge.Profile.Id)).ResultObject;
            currentUser.AnalyticsEnabled = !currentUser.AnalyticsEnabled;
            
            var req = new PrimitiveFieldsUpdateReq<User>(currentUser);
            req.UpdateProperty(nameof(User.AnalyticsEnabled));

            var resp = await Bridge.UpdateAsync(req);
            Debug.Log(JsonConvert.SerializeObject(resp.ResultObject));
        }
    }
}
