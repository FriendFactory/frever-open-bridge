using Bridge.Models.VideoServer;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public class GetTrendingVideoTest : EntityApiTest<Video>
    {
        protected override async void RunTestAsync()
        {
            var videoList = await Bridge.GetTrendingVideoListAsync(null, 10);
            Debug.Log(JsonConvert.SerializeObject(videoList));
        }
    }
}