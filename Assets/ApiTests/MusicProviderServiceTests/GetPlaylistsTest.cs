using UnityEngine;

namespace ApiTests.MusicProviderServiceTests
{
    internal sealed class GetPlaylistsTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var playlistsResponse = await Bridge.GetExternalPlaylists(null, 0, 10);
            if (playlistsResponse != null)
            {
                foreach (var playlist in playlistsResponse.Models)
                {
                    Debug.Log($"Downloaded playlist name: {playlist.Title}");
                }
            }
            else
            {
                Debug.LogError("Failed to get playLists");
            }
        }
    }
}
