using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    internal sealed class GetAgeConfirmationTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetAgeConfirmationQuestions();
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}