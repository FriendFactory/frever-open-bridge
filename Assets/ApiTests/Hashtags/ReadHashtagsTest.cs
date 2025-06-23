using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.Hashtags
{
    public class ReadHashtagsTest : AuthorizedUserApiTestBase
    {
        public string Filter = "S";
        public int Take = 10;
        public int Skip = 0;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetHashtags(Filter, Skip, Take);
            Debug.Log($"Response: {JsonConvert.SerializeObject(resp)}");
        }
    }
}
