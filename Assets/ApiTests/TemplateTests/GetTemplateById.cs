using System.Linq;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TemplateTests
{
    public class GetTemplateById: EntityApiTest<Template>
    {
        public long TemplateId;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetEventTemplate(TemplateId);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}