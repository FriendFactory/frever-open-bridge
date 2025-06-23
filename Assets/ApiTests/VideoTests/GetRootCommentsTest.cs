using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public sealed class GetRootCommentsTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var videoList = await Bridge.GetFeedVideoListAsync(string.Empty, 20);
            foreach (var video in videoList.Models)
            {
                var videoId = video.Id;
                var comments = await Bridge.GetVideoComments(videoId, 1, 0);
                if (comments.IsError)
                {
                    Debug.LogError(comments.ErrorMessage);
                    break;
                }

                if (comments.Models.Length != 0)
                {
                    var targetComment = comments.Models.First();
                    var rootComments = await Bridge.GetVideoRootComments(targetComment.VideoId, targetComment.Key, 0, 10);
                    if (rootComments.IsError)
                    {
                        Debug.LogError(rootComments.ErrorMessage);
                    }
                    else
                    {
                        Debug.Log(JsonConvert.SerializeObject(rootComments.Models));
                    }
                }
            }
        }
    }
}