using Bridge.Models.VideoServer;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public class DeleteVideoByLevelIdTest:EntityApiTest<Video>
    {
        public long LevelId;
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.DeleteVideoByLevelId(LevelId);
            Debug.Log(resp.IsSuccess);
        }
    }
}