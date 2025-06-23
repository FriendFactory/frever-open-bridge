using UnityEngine;

namespace ApiTests.MusicProviderServiceTests
{
    internal sealed class GetTrackPreviewClipTest : AuthorizedUserApiTestBase
    {
        public long Id;
        protected override async void RunTestAsync()
        {
            var trackPreviewClip = await Bridge.DownloadExternalTrackClip(Id);
            if (trackPreviewClip != null)
            {
                Debug.Log($"Track preview clip {Id} downloaded");
            }
            else
            {
                Debug.LogError("Failed to download preview clip.");
            }
        }
    }
}
