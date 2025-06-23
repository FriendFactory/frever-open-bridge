using System.Linq;
using Bridge.Models.AsseManager;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.TemplateTests
{
    public class GetTemplateVideoTest: EntityApiTest<Template>
    {
        protected override async void RunTestAsync()
        {
            var trendingTemplates = await Bridge.GetTrendingEventTemplates(10, 0);
            if (trendingTemplates.IsError)
            {
                Debug.LogError(trendingTemplates.ErrorMessage);
                return;
            }

            if (trendingTemplates.Models.Length == 0)
            {
                Debug.LogError("There is no any template available");
                return;
            }

            var template = trendingTemplates.Models.First();
            var url = Bridge.GetTemplateVideoUrl(template);
            Debug.Log($"Thumbnail url {url}");
        }
    }
}