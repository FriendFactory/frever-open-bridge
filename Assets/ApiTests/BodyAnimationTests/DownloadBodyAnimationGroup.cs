using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.BodyAnimationTests
{
    internal sealed class DownloadBodyAnimationGroup: AuthorizedUserApiTestBase
    {
        public long GroupId;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetBodyAnimationGroupAsync(GroupId);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}