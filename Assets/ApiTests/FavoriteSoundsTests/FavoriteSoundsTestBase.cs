using System.Threading.Tasks;
using Bridge.Models.ClientServer.Assets;
using UnityEngine;

namespace ApiTests.FavoriteSoundsTests
{
    public abstract class FavoriteSoundsTestBase: AuthorizedUserApiTestBase
    {
        protected const int TAKE_COUNT = 50;
        protected const SoundType SOUND_TYPE = SoundType.Song;
        protected const int SONG_ID = 117;
        
        protected async Task<FavouriteMusicInfo[]> GetFavoriteSoundsAsync(int take, int skip = 0)
        {
            var result = await Bridge.GetFavouriteSoundList(take, skip, false, default);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get favorite sounds # {result.ErrorMessage}");
                return null;
            }

            return result.Models;
        }

        protected async Task<bool> TryAddFavoriteSoundAsync(SoundType soundType, long id)
        {
            var result = await Bridge.AddSoundToFavouriteList(soundType, id);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to add favorite sound # {result.ErrorMessage}");
                return false;
            }

            return true;
        }
        
        protected async Task<bool> TryRemoveFavoriteSoundAsync(SoundType soundType, long id)
        {
            var result = await Bridge.RemoveSoundFromFavouriteList(soundType, id);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to remove favorite sound # {result.ErrorMessage}");
                return false;
            }

            return true;
        }
    }
}