using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VideoTests
{
    internal sealed class GetVideoShareInfoTest: AuthorizedUserApiTestBase
    {
        public string VideoGuid;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetVideoShareInfo(VideoGuid);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}