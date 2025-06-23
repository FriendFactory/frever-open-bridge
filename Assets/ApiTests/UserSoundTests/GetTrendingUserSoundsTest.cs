
using UnityEngine;

namespace ApiTests.UserSoundTests
{
    public class GetTrendingUserSoundsTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var response = await Bridge.GetTrendingUserSoundsAsync("",20);
            if (response.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get trending user sounds. Reason: {response.ErrorMessage}");
                return;
            }

            Debug.Log($"[{GetType().Name}] Number of trending sounds: {response.Models?.Length}");
        }
    }
}