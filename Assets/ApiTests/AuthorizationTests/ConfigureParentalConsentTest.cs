using Bridge.Authorization.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    internal sealed class ConfigureParentalConsentTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.ConfigureParentalConsent(new ConfigureParentalConsentRequest()
            {
                ParentalConsent = new ParentalConsent
                {
                    AccessCamera = true
                },
            });
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}