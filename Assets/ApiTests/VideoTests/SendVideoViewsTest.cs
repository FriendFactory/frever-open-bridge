using System;
using System.Linq;
using Bridge.VideoServer;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.VideoTests
{
    public sealed class SendVideoViewsTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private string _videoKey;
        [SerializeField] private string _feedTab = "Trending";
        [SerializeField] private string _feedType = "Featured";
        
        protected override async void RunTestAsync()
        {
            var video = await Bridge.GetFeaturedVideoListAsync(_videoKey, 10);

            if (video.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get videos # {video.ErrorMessage}");
                return;
            }

            var views = video.Models.Select(t => new VideoView()
                {
                    VideoId = t.Id, ViewDate = DateTime.UtcNow, FeedTab = _feedTab, FeedType = _feedType,
                })
                .ToList();

            var result = await Bridge.SendViewsData(views);
            
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to send views # {result.ErrorMessage}");
                return;
            }
            
            Assert.IsTrue(result.IsSuccess);
        }
    }
}