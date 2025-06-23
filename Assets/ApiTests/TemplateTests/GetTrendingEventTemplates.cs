using Bridge.Models.AsseManager;
using Bridge.Models.ClientServer.Template;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TemplateTests
{
    internal sealed class GetTrendingEventTemplates : EntityApiTest<TemplateInfo>
    {
        public int Top = 10;
        public int Skip = 1;
    
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetTrendingEventTemplates(Top, Skip);
            if (resp.IsError)
            {
                Debug.LogError(resp.ErrorMessage);
                return;
            }
        
            Debug.Log($"Total received: {resp.Models.Length}");
            Debug.Log($"Response: {JsonConvert.SerializeObject(resp.Models)}");
        }
    }
}
