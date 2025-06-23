using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common.Files;
using Bridge.Results;
using UnityEngine;

namespace Bridge.ClientServer.ImageGeneration
{
    internal interface IImageGenerationService: IKlingImageGenerationService
    {
        Task<Result<StabilityCreateImageResponse>> GenerateImage(CreateImageRequest req);
        Task<Result<ReplicateResultResponse>> GenerateImage(ReplicateRequest req, CancellationToken token);
        Task<Result<TransformationResponse>> GenerateAiImage(List<byte[]> images, string prompts);
        Task<Result<TransformationResponse>> GenerateAiImage(string prompts);
        Task<Result<TransformationResponse>> GenerateAiImage(string key, byte[] referenceStyleImage);
        Task<Result<TransformationResponse>> GenerateAiImage(byte[] sourceImage, byte[] referenceStyleImage);
        Task<Result<Texture2D>> GetGeneratedAiImageByKey(string key, float timeOut = 15);
        Task<Result<Texture2D>> GetGeneratedAiImageByUrl(string url, float timeOut = 15);
        Task<Result<PhotoTransformationFileInfo>> GetGeneratedAiImagesUrls(string key, CancellationToken token);
        Task<Result<byte[]>> GetGeneratedAiImageBytes(string key, float timeOut = 15);
        Task<Result<byte[]>> GetImageBytes(GeneratedImage model, CancellationToken token = default);
        Task<Result<Texture2D>> GetImage(GeneratedImage model, bool readWrite, CancellationToken token = default);
        Task<ArrayResult<MakeUp>> GetMakeUpList(int skip, int take, CancellationToken token = default);
        Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, string targetImageKey);
        Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, byte[] targetImage);
        Task<Result> SaveGeneratedImage(string key);
        Task<Result> SaveGeneratedImage(FileInfo imageFileInfo);
        Task<ArrayResult<GeneratedImage>> GetUserImages(long groupId, int take, int skip, CancellationToken token);
    }
}