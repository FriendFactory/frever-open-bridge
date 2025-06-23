using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ApiTests.ImageGeneration
{
    internal sealed class DownloadGeneratedImage: AuthorizedUserApiTestBase
    {
        [SerializeField] private RawImage _rawImage;
        
        protected override async void RunTestAsync()
        {
            var generated = await Bridge.GetUserImages(10, 0);
            if (generated.IsError)
            {
                Debug.LogError("Failed to get list");
                return;
            }

            var imageModel = generated.Models.First();
            var resp = await Bridge.GetAssetAsync(imageModel);
            Debug.Log($"Resp success: {resp.IsSuccess}");
            _rawImage.texture = resp.Object as Texture2D;
        }
    }
}