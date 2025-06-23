using Bridge.Models.VideoServer;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public class DeleteVideoTest: EntityApiTest<Video>
    {
        public long Id;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.DeleteVideo(Id);
            Debug.Log(resp.IsSuccess);
        }
    }
}