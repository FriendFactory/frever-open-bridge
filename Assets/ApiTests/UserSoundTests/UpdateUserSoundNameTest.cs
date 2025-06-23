using Bridge.Models.ClientServer.Assets;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.UserSoundTests
{
    public class UpdateUserSoundNameTest: EntityApiTest<UserSoundFullInfo>
    {
        protected override async void RunTestAsync()
        {
            var userSoundsResponse = await Bridge.GetUserSoundsAsync(1, 0);
            if (userSoundsResponse.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get user sounds # {userSoundsResponse.ErrorMessage}");
                return;
            }

            var userSounds = userSoundsResponse.Models;
            if (userSounds.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}] User has no any sound uploaded");
                return;
            }

            var userSound = userSounds[0];
            var name = $"name {Random.Range(1000, 3000)}";

            var updateResponse = await Bridge.UpdateUserSoundNameAsync(userSound.Id, name);
            if (userSoundsResponse.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to update sound name # {userSoundsResponse.ErrorMessage}");
                return;
            }
            
            Assert.AreEqual(name, updateResponse.Model?.Name);
        }
    }
}