using System.Linq;
using UnityEngine;

namespace ApiTests.ExternalSongTests
{
    public sealed class DownloadExternalSongThumbnail : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var songs = await Bridge.SearchExternalTracksAsync("La", 1);
            var randomSong = songs.Models.First();
            var thumbnailRes = await Bridge.DownloadExternalTrackThumbnail(randomSong.ThumbnailUrl);
            Debug.Log(thumbnailRes.IsSuccess);
        }
    }
}
