using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    internal sealed class GetVideoSharingInfoTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private long _videoId;
        
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetVideoSharingInfo(_videoId);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get video sharing info # {result.ErrorMessage}");
                return;
            }
            
            Debug.Log(JsonConvert.SerializeObject(result.Model));
        }
    }
}