using Bridge.Models;
using Bridge.Models.VideoServer;
using Bridge.VideoServer;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public sealed class UploadNonLevelVideoTest: EntityApiTest<Video>
    {
        protected override async void RunTestAsync()
        {
            var videoPath = GetFilePath("sample-video.mp4");

            var deployVideoData = new DeployNonLevelVideoReq(videoPath, 10, false, ServerConstants.VideoPublishingType.STANDARD);
            var uploadResp = await Bridge.UploadNonLevelVideoAsync(deployVideoData);

            Debug.Log(JsonConvert.SerializeObject(uploadResp));
        }
    }
}