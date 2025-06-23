/*
using System.Collections.Generic;
using Bridge.Services._7Digital.QueryModels;
using UnityEngine;

namespace ApiTests.MusicProviderServiceTests
{
    public class CreatePlaylistTest : AuthorizedUserApiTestBase
    {
        public string Name;

        protected override async void RunTestAsync()
        {
            var playListData = new PlaylistData
            {
                name = Name,
                tracks = new []{new TrackData { trackId = 86729797 }, new TrackData { trackId = 86729798 }}
            };
        
            var createPlaylistResponse = await Bridge.CreatePlaylist(playListData);
            if (createPlaylistResponse.Contains("playlist"))
            {
                Debug.Log($"Playlist created {createPlaylistResponse} downloaded");
            }
            else
            {
                Debug.LogError($"Failed to create playList [Reason]: {createPlaylistResponse}");
            }
        }
    }
}
*/
