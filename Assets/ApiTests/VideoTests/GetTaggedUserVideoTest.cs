using Bridge.Models.VideoServer;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public sealed class GetTaggedUserVideoTest : EntityApiTest<Video>
    {
        protected override async void RunTestAsync()
        {
            var videoList = await Bridge.GetUserTaggedVideoListAsync(Bridge.Profile.GroupId, null, 20);
            Debug.Log(JsonConvert.SerializeObject(videoList));
        }
    }
}