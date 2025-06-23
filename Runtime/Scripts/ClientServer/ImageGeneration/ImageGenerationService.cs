using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using BestHTTP.Forms;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService;
using Bridge.Services.AssetService.Caching;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.ClientServer.ImageGeneration
{
    //todo: remove duplicated code by extracting methods
    internal sealed class ImageGenerationService : ServiceBase, IImageGenerationService
    {
        private readonly StabilityImageGenerationService _stabilityImageGenerationService;
        private readonly ReplicateImageGenerationService _replicateImageGenerationService;
        private readonly IKlingImageGenerationService _klingImageGenerationService;
        private readonly IAssetService _assetService;
        
        private readonly AnimationCurve _downloadIntervalCurve;
        private readonly string _aiImageServerHost;

        public int MaxImageSizeKb => _klingImageGenerationService.MaxImageSizeKb;
        public Vector2Int MinImageResolution => _klingImageGenerationService.MinImageResolution;
        
        public ImageGenerationService(string host, string aiImageServerHost, IRequestHelper requestHelper, ISerializer serializer, TempFileCache tempFileCache, IKlingImageGenerationService klingImageGenerationService, IAssetService assetService) : base(host, requestHelper, serializer)
        {
            _aiImageServerHost = aiImageServerHost;
            _klingImageGenerationService = klingImageGenerationService;
            _assetService = assetService;
            _stabilityImageGenerationService = new StabilityImageGenerationService(host, requestHelper, serializer, tempFileCache);
            _replicateImageGenerationService = new ReplicateImageGenerationService(host, requestHelper, serializer, tempFileCache);
            
            //image is expected to be ready within 2 seconds
            _downloadIntervalCurve = new AnimationCurve();
            _downloadIntervalCurve.AddKey(0, 0.4f);
            _downloadIntervalCurve.AddKey(1, 0.2f);
            _downloadIntervalCurve.AddKey(2, 0.1f);
            _downloadIntervalCurve.AddKey(4, 0.1f);
            _downloadIntervalCurve.AddKey(7, 0.5f);
            _downloadIntervalCurve.AddKey(10, 1f);
        }

        public Task<Result<StabilityCreateImageResponse>> GenerateImage(CreateImageRequest req)
        {
            return _stabilityImageGenerationService.GenerateImage(req);
        }

        public Task<Result<ReplicateResultResponse>> GenerateImage(ReplicateRequest req, CancellationToken token)
        {
            return _replicateImageGenerationService.GenerateImage(req, token);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(List<byte[]> images, string prompts)
        {
            var endPoint = $"photo/transformation/image-to-image";
            var files = new Dictionary<string, byte[]>();
            for (var i = 0; i < images.Count; i++)
            {
                files[$"file{i}.jpg"] = images[i];
            }
            return SendGenerateImageRequest(files, "files", endPoint, prompts);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(string prompts)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost ,$"photo/transformation/text-to-image");
            var body = new { PromptText = prompts };
            return SendPostRequest<TransformationResponse>(url, body);
        }

        public Task<Result<TransformationResponse>> GenerateAiImage(string key, byte[] referenceStyleImage)
        {
            var endPoint = $"photo/transformation/style?key={key}";
            var files = new Dictionary<string, byte[]>
            {
                { "sourceFile.jpg", referenceStyleImage }
            };
            return SendGenerateImageRequest(files, "sourceFile", endPoint);
        }

        public async Task<Result<TransformationResponse>> GenerateAiImage(byte[] sourceImage, byte[] referenceStyleImage)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"photo/transformation/style");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
           
            var form = new HTTPMultiPartForm();
            form.AddBinaryData("inputFile", sourceImage, "inputFile.jpg");
            form.AddBinaryData("sourceFile", referenceStyleImage, "sourceFile.jpg");
            request.SetForm(form);
            
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<TransformationResponse>.Error(resp.DataAsText, resp.StatusCode);
            }

            var model = Serializer.DeserializeJson<TransformationResponse>(resp.DataAsText);
            return Result<TransformationResponse>.Success(model);
        }

        public Task<Result<Texture2D>> GetGeneratedAiImageByKey(string key, float timeOut)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"photo/transformation/result?key={key}");
            return DownloadImageInternal(url, timeOut, true);
        }

        public Task<Result<Texture2D>> GetGeneratedAiImageByUrl(string url, float timeOut = 15)
        {
            return DownloadImageInternal(url, timeOut, false);
        }

        public Task<Result<PhotoTransformationFileInfo>> GetGeneratedAiImagesUrls(string key, CancellationToken token = default)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"/photo/transformation/result/urls?key={key}");
            return SendRequestForSingleModel<PhotoTransformationFileInfo>(url, token);
        }

        public async Task<Result<byte[]>> GetGeneratedAiImageBytes(string key, float timeOut)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"photo/transformation/result?key={key}");
            
            const float attemptIntervalSec = 0.5f;
            var maxAttempts = Mathf.CeilToInt(timeOut / attemptIntervalSec);

            for (int i = 0; i < maxAttempts; i++)
            {
                using var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
                request.OnBeforeRedirection += (originalRequest, response, uri) => originalRequest.RemoveHeader("Authorization");
                var resp = await request.GetHTTPResponseAsync();
                if (!resp.IsSuccess)
                {
                    var isLastAttempt = i == maxAttempts - 1;
                    if (isLastAttempt)
                    {
                        return Result<byte[]>.Error(request.Response.StatusCode.ToString());
                    }
                    await Task.Delay((int)TimeSpan.FromSeconds(attemptIntervalSec).TotalMilliseconds);
                    continue;
                }
                
                return Result<byte[]>.Success(resp.Data);
            }
            
            return Result<byte[]>.Error("TIME_OUT");
        }

        public async Task<Result<byte[]>> GetImageBytes(GeneratedImage model, CancellationToken token = default)
        {
            try
            {
                return await GetImageBytesInternal(model, token);
            }
            catch (Exception e)
            {
                return Result<byte[]>.Error(e.Message);
            }
        }

        public async Task<Result<Texture2D>> GetImage(GeneratedImage model, bool readWrite, CancellationToken token = default)
        {
            if (!readWrite)
            {
                //returns faster, since it does not cache before getting repsonse
                var resp = await _assetService.GetAssetAsync(model, model.Files.First(), true, token);
                if (resp.IsSuccess) return Result<Texture2D>.Success(resp.Object as Texture2D);
                return resp.IsRequestCanceled ? Result<Texture2D>.Cancelled() : Result<Texture2D>.Error(resp.ErrorMessage);
            }
            
            //todo: optimize to provide bytes from RAM without placing on disk
            var bytesResp = await GetImageBytes(model, token);
            if (bytesResp.IsSuccess)
            {
                return Result<Texture2D>.Success(LoadReadableTexture(bytesResp.Model));
            }
            return bytesResp.IsRequestCanceled ? Result<Texture2D>.Cancelled() : Result<Texture2D>.Error(bytesResp.ErrorMessage);
        }

        public Task<ArrayResult<MakeUp>> GetMakeUpList(int skip, int take, CancellationToken token = default)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"photo/transformation/make-up?skip={skip}&take={take}");
            return SendRequestForListModels<MakeUp>(url, token);
        }

        public Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, string targetImageKey)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"photo/transformation/make-up/{makeupId}?key={targetImageKey}");
            return SendPostRequest<TransformationResponse>(url);
        }

        public async Task<Result<TransformationResponse>> ApplyMakeUp(long makeupId, byte[] targetImage)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"photo/transformation/make-up/{makeupId}/file");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            
            var form = new HTTPMultiPartForm();
            form.AddBinaryData("file", targetImage);
            request.SetForm(form);
        
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<TransformationResponse>.Error(resp.DataAsText, resp.StatusCode);
            }

            var model = Serializer.DeserializeJson<TransformationResponse>(resp.DataAsText);
            return Result<TransformationResponse>.Success(model);
        }

        public Task<Result> SaveGeneratedImage(string key)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"picture/transformed?key={key}");
            return SendPostRequest(url);
        }

        public async Task<Result> SaveGeneratedImage(FileInfo imageFileInfo)
        {
            var preuploadFileResp = await _assetService.UploadFileAsync(imageFileInfo, default);
            if (preuploadFileResp.IsError)
            {
                return new ErrorResult($"Failed to upload file:{preuploadFileResp.ErrorMessage}");
            }

            var url = Extensions.CombineUrls(_aiImageServerHost, "picture");
            return await SendPostRequest(url, new
            {
                Files = new List<FileInfo> { imageFileInfo }
            });
        }

        public Task<ArrayResult<GeneratedImage>> GetUserImages(long groupId, int take, int skip, CancellationToken token)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, $"picture/{groupId}?take={take}&skip={skip}");
            return SendRequestForListModels<GeneratedImage>(url, token);
        }
        
        private async Task<Result<TransformationResponse>> SendGenerateImageRequest(IEnumerable<KeyValuePair<string, byte[]>> files, string filePropertyName, string endPoint, string promptText = null)
        {
            var url = Extensions.CombineUrls(_aiImageServerHost, endPoint);
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
           
            var form = new HTTPMultiPartForm();
            foreach (var nameAndFile in files)
            {
                form.AddBinaryData(filePropertyName, nameAndFile.Value, nameAndFile.Key);
            }
            
            if (!string.IsNullOrEmpty(promptText))
            {
                form.AddField("PromptText", promptText);
            }
            request.SetForm(form);
            
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<TransformationResponse>.Error(resp.DataAsText, resp.StatusCode);
            }

            var model = Serializer.DeserializeJson<TransformationResponse>(resp.DataAsText);
            return Result<TransformationResponse>.Success(model);
        }

        public Task<Result<ScheduleTryOnOutfitResponse>> ScheduleTryOnWardrobeTask(ScheduleTryOnOutfitRequest requestModel)
        {
            return _klingImageGenerationService.ScheduleTryOnWardrobeTask(requestModel);
        }

        public Task<Result<TaskStatusResponse>> GetImageGenerationTaskStatus(string taskId, CancellationToken token)
        {
            return _klingImageGenerationService.GetImageGenerationTaskStatus(taskId, token);
        }
        
        private async Task<Result<byte[]>> GetImageBytesInternal(GeneratedImage model, CancellationToken token = default)
        {
            var fileInfo = model.Files.First();
            
            var fetch = await _assetService.Fetch(model, fileInfo, token);
            if (fetch.IsError)
            {
                return Result<byte[]>.Error(fetch.ErrorMessage);
            }

            var filePath = fetch.FilePath;
            return Result<byte[]>.Success(await File.ReadAllBytesAsync(filePath, token));
        }
        
        private static Texture2D LoadReadableTexture(byte[] imageData)
        {
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            return texture.LoadImage(imageData) ? texture : null;
        }
        
        private async Task<Result<Texture2D>> DownloadImageInternal(string url, float timeOut, bool addToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            do
            {
                using var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, addToken, false);
                if (addToken)
                {
                    request.OnBeforeRedirection += (originalRequest, response, uri) => originalRequest.RemoveHeader("Authorization");
                }
                var resp = await request.GetHTTPResponseAsync();
                if (!resp.IsSuccess)
                {
                    var interval = _downloadIntervalCurve.Evaluate(stopwatch.Elapsed.Milliseconds / 1000f);

                    var isLastAttempt = stopwatch.Elapsed + TimeSpan.FromSeconds(interval) >= TimeSpan.FromSeconds(timeOut);
                    if (isLastAttempt)
                    {
                        stopwatch.Stop();
                        return Result<Texture2D>.Error(request.Response.StatusCode.ToString());
                    }
                    
                    await Task.Delay((int)TimeSpan.FromSeconds(interval).TotalMilliseconds);
                    continue;
                }
                
                stopwatch.Stop();
                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.LoadImage(resp.Data, false);
                return Result<Texture2D>.Success(texture);
            } while (stopwatch.Elapsed.TotalSeconds < timeOut);
            
            stopwatch.Stop();
            return Result<Texture2D>.Error("TIME_OUT");
        }
    }
}