using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Models.VideoServer;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TemplateTests
{
    public class GetVideoForTemplate: EntityApiTest<Video>
    {
        public bool UseAnyAvaiableTemplate;
        [DrawIf(nameof(UseAnyAvaiableTemplate), false)]
        public long TemplateId;
        public string VideoKey;
        public int TakeNext;
        
        protected override async void RunTestAsync()
        {
            TemplateId = UseAnyAvaiableTemplate ? await GetRandomTemplate() : TemplateId;
            if (UseLastSavedUser && Environment != Bridge.Environment)
            {
                Debug.LogError("Environment for last saved user is not the same as target for test");
                return;
            }
            var resp = await Bridge.GetVideoForTemplate(TemplateId, VideoKey, TakeNext);
            if (resp.IsSuccess)
            {
                Debug.Log($"Response: {JsonConvert.SerializeObject(resp.Models.Length)}");
            }
            else
            {
                Debug.LogError(resp.ErrorMessage);
            }
        }

        private async Task<long> GetRandomTemplate()
        {
            var resp = await Bridge.GetTrendingEventTemplates(1, 0);
            if (resp.IsSuccess)
                return resp.Models.First().Id;
            throw new Exception($"Failed getting template id: {resp.ErrorMessage}");
        }
    }
}