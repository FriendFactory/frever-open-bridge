using System;
using System.Linq;
using Bridge.Models.ClientServer.Assets;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.FavoriteSoundsTests
{
    public class AddFavoriteSoundTest: FavoriteSoundsTestBase 
    {
        protected override async void RunTestAsync()
        {
            try
            {
                var result = await Bridge.AddSoundToFavouriteList(SoundType.UserSound, 234);
                if (result.IsError)
                {
                    Debug.LogError($"Failed to add favorite sound # {result.ErrorMessage}");
                    return;
                }

                var favoriteSound = result.Model;
                var favoriteSounds = await GetFavoriteSoundsAsync(TAKE_COUNT);
                
                if (favoriteSounds == null) return;
                
                Assert.IsTrue(favoriteSounds.Any(sound => sound.Id == favoriteSound.Id));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}