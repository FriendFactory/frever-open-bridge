using Bridge.Models.VideoServer;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TemplateTests
{
    public class GetTemplateChallengesTest: EntityApiTest<TemplateChallenge>
    {
        public int Take=10;
        public int Skip;
        public int VideoPerTemplate;
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetTrendingTemplateChallenges(Take, Skip, VideoPerTemplate);
            Debug.Log($"Response: {JsonConvert.SerializeObject(resp)}");
        }
    }
}