using System.Linq;
using Bridge.Models.ClientServer.Level.Shuffle;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.LevelTests
{
    internal sealed class GetLevelMlShuffledTest : AuthorizedUserApiTestBase
    {
        [SerializeField] private string _text;
        
        protected override async void RunTestAsync()
        {
            var videoResult = await Bridge.GetForYouFeedVideoListAsync(null, 10);
            if (videoResult.IsError)
            {
                Debug.LogError(videoResult.ErrorMessage);
                return;
            }

            var video = videoResult.Models.First(x => x.IsRemixable);
            var resp = await Bridge.GetShuffledLevelRemixAI(new MlRemixRequest
            {
                VideoId = video.Id,
                Text = _text
            });
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}