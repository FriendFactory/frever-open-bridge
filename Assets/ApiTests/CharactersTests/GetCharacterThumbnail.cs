using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.CharactersTests
{
    public class GetCharacterThumbnail: EntityApiTest<Character>
    {
        protected override async void RunTestAsync()
        {
            var getReps = await Bridge.GetAsync(new Query<Character>());
            var download = await Bridge.GetThumbnailAsync(getReps.Models.Last(), Resolution._128x128);
            if (download.IsSuccess)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError(download.ErrorMessage);
            }
        }
    }
}