using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ContentModerationTests
{
    internal sealed class AnalyzeVideoTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var filePath = GetFilePath(TestFileNames.VIDEO_MP4);
            var fileExtension = Path.GetExtension(filePath).Replace(".", string.Empty);
            var result = await Bridge.ModerateMediaContent(filePath, fileExtension);
            Debug.Log(JsonConvert.SerializeObject(result));
        }
    }
}