using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.Emotions
{
    internal sealed class GetEmotionAssetsSetupTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var emotionsModelsResult = await Bridge.GetEmotionsAsync(20, 0);
            var emotion = emotionsModelsResult.Models.ElementAt(Random.Range(0, emotionsModelsResult.Models.Length - 1));
            var assetsSetupResult = await Bridge.GetEmotionAssetsSetupAsync(emotion.Id);
            Debug.Log($"### {JsonConvert.SerializeObject(assetsSetupResult)}");
        }
    }
}
