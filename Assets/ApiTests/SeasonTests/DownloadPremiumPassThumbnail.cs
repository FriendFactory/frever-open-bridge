using System.Linq;
using System.Threading;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.SeasonTests
{
    internal sealed class DownloadPremiumPassThumbnail: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var productsResp = await Bridge.GetProductOffers();
            if (productsResp.IsError)
            {
                Debug.LogError($"Failed to load products. Reason: {productsResp.ErrorMessage}");
                return;
            }

            var seasonPass = productsResp.Model.InAppProducts.FirstOrDefault(x => x.Offer.IsSeasonPass);
            if (seasonPass == null)
            {
                Debug.LogError("There is no season pass product");
                return;
            }

            var thumbnailResp = await Bridge.GetThumbnailAsync(seasonPass.Offer, Resolution._1024x1024, false,
                CancellationToken.None);
            if (thumbnailResp.IsSuccess)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError($"Failed to load image. Reason: {thumbnailResp.ErrorMessage}");
            }
        }
    }
}