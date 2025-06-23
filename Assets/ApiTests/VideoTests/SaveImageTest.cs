using Bridge.Models.Common.Files;
using UnityEngine;

namespace ApiTests.VideoTests
{
    internal sealed class SaveImageTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private Texture2D _texture;
        
        protected override async void RunTestAsync()
        {
            var fileInfo = new FileInfo(_texture, FileExtension.Jpg, fileType: FileType.MainFile);
            var resp = await Bridge.SaveGeneratedImage(fileInfo);
            Debug.Log($"### Result: {resp.IsSuccess}");
        }
    }
}