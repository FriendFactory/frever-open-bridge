using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer.ImageGeneration;
using Bridge.Models.Common.Files;
using Bridge.Results;
using UnityEngine;

namespace Bridge
{
    public interface IImageGenerationBridge
    {
        Task<Result<StabilityCreateImageResponse>> GenerateImage(CreateImageRequest req);
        Task<Result<ReplicateResultResponse>> GenerateImage(ReplicateRequest req, CancellationToken token = default);
        Task<Result<byte[]>> GetImageBytes(GeneratedImage model, CancellationToken token = default);
        Task<Result<Texture2D>> GetImage(GeneratedImage model, bool readWrite, CancellationToken token = default);

        #region Comfy UI
        Task<Result<TransformationResponse>> GenerateAiImage(string prompts);
        Task<Result<TransformationResponse>> GenerateAiImage(string key, byte[] referenceStyleImage);
        Task<Result<TransformationResponse>> GenerateAiImage(byte[] sourceImage, byte[] referenceStyleImage);
        Task<Result<TransformationResponse>> GenerateAiImage(List<byte[]> images, string prompts);
        Task<Result<PhotoTransformationFileInfo>> GetGeneratedAiImagesUrls(string key, CancellationToken token);
        Task<Result<Texture2D>> GetGeneratedAiImageByKey(string key, float timeOut = 15);
        Task<Result<Texture2D>> GetGeneratedAiImageByUrl(string url, float timeOut = 15);
        Task<Result<byte[]>> GetGeneratedAiImageBytes(string key, float timeOut = 15);
        Task<ArrayResult<MakeUp>> GetMakeUpList(int skip, int take, CancellationToken token = default);
        Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, string targetImageKey);
        Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, byte[] targetImage);
        Task<Result> SaveGeneratedImage(string key);
        Task<Result> SaveGeneratedImage(FileInfo imageFileInfo);
        Task<ArrayResult<GeneratedImage>> GetUserImages(long groupId, int take, int skip, CancellationToken token = default);
        Task<ArrayResult<GeneratedImage>> GetUserImages(int take, int skip, CancellationToken token = default);
        #endregion
        
        #region Kling service
        int MaxImageSizeKb { get; }
        Vector2Int MinImageResolution { get; }
        Task<Result<ScheduleTryOnOutfitResponse>> ScheduleTryOnWardrobeTask(ScheduleTryOnOutfitRequest requestModel);
        Task<Result<TaskStatusResponse>> GetImageGenerationTaskStatus(string taskId, CancellationToken token = default);
        #endregion
       }
}