using ApiTests;
using Newtonsoft.Json;
using UnityEngine;

public class GetCommentsTest : AuthorizedUserApiTestBase
{
    public int Take = 10;
    public int Skip = 0;
    
    protected override async void RunTestAsync()
    {
        var videoListResult = await Bridge.GetTrendingVideoListAsync(null, 10);
        
        foreach (var video in videoListResult.Models)
        {
            var comments = await Bridge.GetVideoComments(video.Id, Take, Skip);
            if (comments.IsSuccess && comments.Models.Length != 0)
            {
                Debug.Log(JsonConvert.SerializeObject(comments.Models));
            }else if (comments.IsError)
            {
                Debug.LogError(comments.ErrorMessage);
            }
            else
            {
                Debug.Log("Video does not have comments");
            }
        }
        
    }
}
