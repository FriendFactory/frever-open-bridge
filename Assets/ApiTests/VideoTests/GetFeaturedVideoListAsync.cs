using System.Linq;
using Bridge.Models.VideoServer;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public class GetFeaturedVideoListAsync: EntityApiTest<Video>
    {
        public int TakeNext = 10;
        public string VideoKey;
        
        protected override async void RunTestAsync()
        {
            var response = await Bridge.GetFeaturedVideoListAsync(VideoKey, TakeNext);
            Debug.Log(response);
            response = await Bridge.GetFeaturedVideoListAsync(response.Models.Last().Key, TakeNext);
            Debug.Log(JsonConvert.SerializeObject(response));
        }
    }
}