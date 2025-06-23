using System.Collections.Generic;
using Bridge.ClientServer.ImageGeneration;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ImageGeneration
{
    internal sealed class ImageGenerationTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var req = new CreateImageRequest
            {
                Engine = Engine.SD_v1_6,
                Width = 1024,
                Height = 1024,
                CfgScale = 1,
                Steps = 12,
                TextPrompts = new List<TextPrompt>
                {
                    new TextPrompt
                    {
                        Text = "Winter",
                        Weight = 0.3f
                    },
                    new TextPrompt
                    {
                        Text = "Animals",
                        Weight = 0.7f
                    }
                }
            };
            var resp = await Bridge.GenerateImage(req);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}
