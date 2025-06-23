using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TemplateTests
{
    internal sealed class GetLevelTemplate: AuthorizedUserApiTestBase
    {
        [SerializeField] private long TemplateId;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetEventForEventTemplate(TemplateId);
            Debug.Log(JsonConvert.SerializeObject(resp.Model));
        }
    }
}