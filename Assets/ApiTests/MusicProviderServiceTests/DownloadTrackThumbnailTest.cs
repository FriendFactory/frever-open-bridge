using UnityEngine;

namespace ApiTests.MusicProviderServiceTests
{
    internal sealed class DownloadTrackThumbnailTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var trackResponse = await Bridge.GetTrackDetails(86729797);
            var thumbnailResponse = await Bridge.DownloadExternalTrackThumbnail(trackResponse.Model.ThumbnailUrl);
            if (thumbnailResponse != null)
            {
                Debug.Log("Downloaded track thumbnail");
            }
            else
            {
                Debug.LogError("Failed to get track thumbnail");
            }
        }
    }
}
