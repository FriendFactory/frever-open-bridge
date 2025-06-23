// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace ApiTests.VideoTests
// {
//     internal sealed class SwapFaceOnPhotoTest: AuthorizedUserApiTestBase
//     {
//         [SerializeField] private RawImage _rawImage;
//         [SerializeField] private Texture2D _originalVideo;
//         [SerializeField] private List<Texture2D> _faces;
//         
//         protected override async void RunTestAsync()
//         {
//             var original = _originalVideo.EncodeToJPG();
//             var replace = _faces.Select(x => x.EncodeToJPG()).ToList();
//             var resp = await Bridge.SwapFace(original, replace);
//             if (!resp.IsSuccess)
//             {
//                 Debug.LogError($"Failed to swap faces: {resp.ErrorMessage}");
//                 return;
//             }
//
//             var imageResp = await Bridge.GetGeneratedAiImage(resp.Model.Key, 30);
//             if (!imageResp.IsSuccess)
//             {
//                 Debug.LogError($"Failed to swap faces: {resp.ErrorMessage}");
//                 return;
//             }
//
//             _rawImage.texture = imageResp.Model;
//         }
//     }
// }