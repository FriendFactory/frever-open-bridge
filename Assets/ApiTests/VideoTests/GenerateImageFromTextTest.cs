using UnityEngine;
using UnityEngine.UI;

namespace ApiTests.VideoTests
{
    internal sealed class GenerateImageFromTextTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private string _prompts;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GenerateAiImage(_prompts);
            if (resp.IsError)
            {
                Debug.LogError($"Failed ({resp.HttpStatusCode}): {resp.ErrorMessage}");
                return;
            }
            Debug.Log($"AI generation initialized successfully. Please wait for file");

            var imageResult = await Bridge.GetGeneratedAiImageByKey(resp.Model.Key, 30);
            if (imageResult.IsError)
            {
                Debug.LogError($"Error: {resp.ErrorMessage}");
                return;
            }
            
            _rawImage.texture = imageResult.Model;
        }
    }
}