using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ApiTests.VideoTests
{
    internal sealed class GetMultiImageResultTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private string _prompts;
        [SerializeField] private RawImage _rawImage;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GenerateAiImage(_prompts);
            if (resp.IsError)
            {
                Debug.LogError($"Failed ({resp.HttpStatusCode}): {resp.ErrorMessage}");
                return;
            }
            Debug.Log($"AI generation initialized successfully. Please wait for file");

            var urlsResult = await Bridge.GetGeneratedAiImagesUrls(resp.Model.Key, default);
            if (urlsResult.IsError)
            {
                Debug.LogError($"Error: {resp.ErrorMessage}");
                return;
            }

            var imageResp = await Bridge.GetGeneratedAiImageByUrl(urlsResult.Model.MainFileUrl);
            if (imageResp.IsError)
            {
                Debug.LogError($"Failed to download file. Error: {resp.ErrorMessage}");
                return;
            }

            _rawImage.texture = imageResp.Model;
        }
    }
}