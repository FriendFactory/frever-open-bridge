using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.Advertasing
{
    public class GetPromotedSongsData : AuthorizedUserApiTestBase
    {
        [SerializeField] private RawImage _target;
    
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetPromotedSongs(10, 0);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get promoted songs # {result.ErrorMessage}");
                return;
            }
        
            Assert.IsNotNull(result.Models);

            if (result.Models.Length == 0)
            {
                Debug.Log($"[{GetType().Name}] List of promoted songs is empty - getting thumbnail test will be skipped");
                return;
            }

            var promotedSong = result.Models[0];

            var bannerResult = await Bridge.GetThumbnailAsync(promotedSong, Resolution._512x512, false);
            if (bannerResult.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get promoted song banner # {result.ErrorMessage}");
                return;
            }

            var texture = (Texture2D)bannerResult.Object;
        
            if (_target)
            {
                _target.texture = texture;
            }

            Assert.IsNotNull(texture);
        }
    }
}
