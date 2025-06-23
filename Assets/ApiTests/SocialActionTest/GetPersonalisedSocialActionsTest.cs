using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.SocialActionTest
{
    public class GetPersonalisedSocialActionsTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetPersonalisedSocialActions();

            if (result.IsError)
            {
                Debug.Log(result.ErrorMessage);
                return;
            }
            
            Debug.Log(JsonConvert.SerializeObject(result.Models));
        }
    }
}