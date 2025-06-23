using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer.ImageGeneration;
using Bridge.Models.Common.Files;
using Bridge.Results;
using UnityEngine;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<Result<StabilityCreateImageResponse>> GenerateImage(CreateImageRequest req)
        {
            return _imageGenerationService.GenerateImage(req);
        }

        public Task<Result<ReplicateResultResponse>> GenerateImage(ReplicateRequest req, CancellationToken token = default)
        {
            return _imageGenerationService.GenerateImage(req, token);
        }

        public Task<Result<byte[]>> GetImageBytes(GeneratedImage model, CancellationToken token = default)
        {
            return _imageGenerationService.GetImageBytes(model, token);
        }

        public Task<Result<Texture2D>> GetImage(GeneratedImage model, bool readWrite, CancellationToken token = default)
        {
            return _imageGenerationService.GetImage(model, readWrite, token);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(string prompts)
        {
            return _imageGenerationService.GenerateAiImage(prompts);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(string key, byte[] referenceStyleImage)
        {
            return _imageGenerationService.GenerateAiImage(key, referenceStyleImage);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(byte[] sourceImage, byte[] referenceStyleImage)
        {
            return _imageGenerationService.GenerateAiImage(sourceImage, referenceStyleImage);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(List<byte[]> images, string prompts)
        {
            return _imageGenerationService.GenerateAiImage(images, prompts);
        }

        public Task<Result<PhotoTransformationFileInfo>> GetGeneratedAiImagesUrls(string key, CancellationToken token)
        {
            return _imageGenerationService.GetGeneratedAiImagesUrls(key, token);
        }

        public Task<Result<Texture2D>> GetGeneratedAiImageByKey(string key, float timeOut = 15)
        {
            return _imageGenerationService.GetGeneratedAiImageByKey(key, timeOut);
        }

        public Task<Result<Texture2D>> GetGeneratedAiImageByUrl(string url, float timeOut = 15)
        {
            return _imageGenerationService.GetGeneratedAiImageByUrl(url, timeOut);
        }

        public Task<Result<byte[]>> GetGeneratedAiImageBytes(string key, float timeOut = 15)
        {
            return _imageGenerationService.GetGeneratedAiImageBytes(key, timeOut);
        }

        public Task<ArrayResult<MakeUp>> GetMakeUpList(int skip, int take, CancellationToken token = default)
        {
            return _imageGenerationService.GetMakeUpList(skip, take, token);
        }

        public Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, string targetImageKey)
        {
            return _imageGenerationService.ApplyMakeUp(makeupId, targetImageKey);
        }

        public Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, byte[] targetImage)
        {
            return _imageGenerationService.ApplyMakeUp(makeupId, targetImage);
        }

        public Task<Result> SaveGeneratedImage(string key)
        {
            return _imageGenerationService.SaveGeneratedImage(key);
        }

        public Task<Result> SaveGeneratedImage(FileInfo imageFileInfo)
        {
            return _imageGenerationService.SaveGeneratedImage(imageFileInfo);
        }

        public Task<ArrayResult<GeneratedImage>> GetUserImages(long groupId, int take, int skip, CancellationToken token)
        {
            return _imageGenerationService.GetUserImages(groupId, take, skip, token);
        }

        public Task<ArrayResult<GeneratedImage>> GetUserImages(int take, int skip, CancellationToken token = default)
        {
            return _imageGenerationService.GetUserImages(Profile.GroupId, take, skip, token);
        }

        public int MaxImageSizeKb => _imageGenerationService.MaxImageSizeKb;
        public Vector2Int MinImageResolution => _imageGenerationService.MinImageResolution;

        public Task<Result<ScheduleTryOnOutfitResponse>> ScheduleTryOnWardrobeTask(ScheduleTryOnOutfitRequest requestModel)
        {
            return _imageGenerationService.ScheduleTryOnWardrobeTask(requestModel);
        }

        public Task<Result<TaskStatusResponse>> GetImageGenerationTaskStatus(string taskId, CancellationToken token = default)
        {
            return _imageGenerationService.GetImageGenerationTaskStatus(taskId, token);
        }
    }
}