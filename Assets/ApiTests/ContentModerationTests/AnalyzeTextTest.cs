using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ContentModerationTests
{
    internal sealed class AnalyzeTextTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var result = await Bridge.ModerateTextContent("Ass");
            Debug.Log(JsonConvert.SerializeObject(result));
        }
    }
}
