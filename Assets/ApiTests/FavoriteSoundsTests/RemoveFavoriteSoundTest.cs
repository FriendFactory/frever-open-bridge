using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.FavoriteSoundsTests
{
    public class RemoveFavoriteSoundTest: FavoriteSoundsTestBase 
    {
        protected override async void RunTestAsync()
        {
            try
            {
                var sounds = await GetFavoriteSoundsAsync(TAKE_COUNT);
                
                if (sounds == null) return;

                var soundType = sounds.Length == 0 ? SOUND_TYPE : sounds[0].Type;
                var soundId = sounds.Length == 0 ? SONG_ID : sounds[0].Id;
                
                if (sounds.Length == 0)
                {
                    if (!await TryAddFavoriteSoundAsync(SOUND_TYPE, TAKE_COUNT)) return;
                }
                
                if (! await TryRemoveFavoriteSoundAsync(soundType, soundId)) return;
                
                sounds = await GetFavoriteSoundsAsync(TAKE_COUNT);
                
                if (sounds == null) return;
                
                Assert.IsTrue(sounds.All(sound => sound.Id != soundId));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}