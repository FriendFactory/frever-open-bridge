using UnityEngine;
using UnityEngine.Video;
using VideoClip = Bridge.Models.AsseManager.VideoClip;

namespace ApiTests.VideoClipTests
{
    [RequireComponent(typeof(VideoPlayer))]
    public class DownloadVideoTest : EntityApiTest<VideoClip>
    {
        protected override async void RunTestAsync()
        {
            var videoId = await GetAnyAvailableEntityId<VideoClip>();
            var videoResponse = await Bridge.GetAsync<VideoClip>(videoId);
            var resp = await Bridge.GetAssetAsync(videoResponse.ResultObject);
            var videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.isLooping = true;
            videoPlayer.url = resp.FilePath;
            videoPlayer.Play();
        }
    }
}
