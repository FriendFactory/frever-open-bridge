using UnityEngine;

namespace ApiTests.MusicProviderServiceTests
{
    internal sealed class SearchTracksTest : AuthorizedUserApiTestBase
    {
        public string TrackName;
    
        protected override async void RunTestAsync()
        {
            var trackSearchResponse = await Bridge.SearchExternalTracksAsync(TrackName);
            if (trackSearchResponse.IsSuccess)
            {
                foreach (var track in trackSearchResponse.Models)
                {
                    Debug.Log($"Searched track name: {track.Title}");
                }
            }
            else
            {
                Debug.LogError($"Failed to search tracks # {trackSearchResponse.ErrorMessage}");
            }
        }
    }
}
