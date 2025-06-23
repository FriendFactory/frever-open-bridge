using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.SongServiceTests
{
    public class GetCommercialSongsOnlyTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resultCommercial = await Bridge.GetSongsAsync(100, 0, commercialOnly: true);
            if (resultCommercial.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get commercial songs # {resultCommercial.ErrorMessage}");
                return;
            }

            var result = await Bridge.GetSongsAsync(100, 0);
            if (resultCommercial.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get songs # {resultCommercial.ErrorMessage}");
                return;
            }

            var commercialSongs = resultCommercial.Models;
            var songs = result.Models;
        
            // SongInfo doesn't contain any is commercial only field
            // probably test will fail with number of songs > 100
            Assert.AreNotEqual(commercialSongs.Length, songs.Length);
        }
    }
}
