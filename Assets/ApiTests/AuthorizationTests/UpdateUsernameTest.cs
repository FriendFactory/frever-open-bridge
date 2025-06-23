using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.AuthorizationTests
{
    public class UpdateUsernameTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private string _username;
        
        protected override async void RunTestAsync()
        {
            try
            {
                var result = await Bridge.UpdateUsername(_username);
                if (!result.Ok)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to update username: {result.ErrorDetails}, {result.ErrorCode}");
                    return;
                }

                Debug.Log(JsonConvert.SerializeObject(result));
                
                Assert.IsTrue(result.UsernameUpdateAvailableOn > DateTime.UtcNow);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}