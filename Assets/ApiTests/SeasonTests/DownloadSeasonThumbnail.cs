using System.Linq;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.SeasonTests
{
    internal sealed class DownloadSeasonThumbnail : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetCurrentSeason();
            var season = resp.Model;
            var marketingScreenshot = season.MarketingScreenshots.FirstOrDefault();
            if (marketingScreenshot == null)
            {
                Debug.LogError($"Season does not have any marketing screenshot");
                return;
            }
            var thumbnailReq = await Bridge.GetThumbnailAsync(marketingScreenshot, Resolution._512x512, false);
            if (thumbnailReq.IsSuccess)
            {
                Debug.Log($"### Downloaded");
            }
            else
            {
                Debug.LogError($"Failed to download: {thumbnailReq.ErrorMessage}");
            }
        }
    }
}
