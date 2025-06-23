using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Newtonsoft.Json;
using UnityEngine;

namespace Bridge.ClientServer.ImageGeneration
{
    internal sealed partial class KlingImageGenerationService : ServiceBase, IKlingImageGenerationService
    {
        private const string END_POINT = "/v1/images/kolors-virtual-try-on";
        
        private readonly string _tokenGenerationHost;
        private string _token;
        private DateTime _expiresAt;
        
        public KlingImageGenerationService(string host, string tokenGenerationHost, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
            _tokenGenerationHost = tokenGenerationHost;
        }

        public int MaxImageSizeKb => 10 * 1000;//10 mb
        public Vector2Int MinImageResolution => new(300, 300);

        public async Task<Result<ScheduleTryOnOutfitResponse>> ScheduleTryOnWardrobeTask(ScheduleTryOnOutfitRequest requestModel)
        {
            var token = await GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return Result<ScheduleTryOnOutfitResponse>.Error("Failed to get access to ai generation service");
            }

            var body = new TryOnRequestBody
            {
                HumanImage = requestModel.HumanImage,
                ClothImage = requestModel.ClothesImage,
                ModelName = "kolors-virtual-try-on-v1-5"
            };
            var url = ConcatUrl(Host, END_POINT);
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, token);
            request.AddJsonContent(JsonConvert.SerializeObject(body));
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<ScheduleTryOnOutfitResponse>.Error($"Failed to get generate. Reason: {resp.StatusCode}. {resp.DataAsText}"); 
            }

            var respModel = Serializer.DeserializeJson<TryOnResponse>(resp.DataAsText);

            if (respModel.Code > 0)
            {
                return Result<ScheduleTryOnOutfitResponse>.Error($"Failed to get generate. Reason: {respModel.Code}. {respModel.Message}"); 
            }

            return Result<ScheduleTryOnOutfitResponse>.Success(new ScheduleTryOnOutfitResponse
            {
                TaskId = respModel.Data.TaskId,
                TaskStatus = respModel.Data.TaskStatus,
            });
        }

        public async Task<Result<TaskStatusResponse>> GetImageGenerationTaskStatus(string taskId, CancellationToken cancellationToken)
        {
            try
            {
                return await GetImageGenerationTaskStatusInternal(taskId, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return Result<TaskStatusResponse>.Cancelled(); 
            }
        }
        
        private async Task<Result<TaskStatusResponse>> GetImageGenerationTaskStatusInternal(string taskId, CancellationToken cancellationToken)
        {
            var accessToken = await GetToken(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return Result<TaskStatusResponse>.Cancelled();
            }
            if (string.IsNullOrEmpty(accessToken))
            {
                return Result<TaskStatusResponse>.Error("Failed to get access to ai generation service");
            }
            
            var url = ConcatUrl(Host, $"{END_POINT}/{taskId}");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, accessToken);

            var resp = await request.GetHTTPResponseAsync(cancellationToken);
            if (!resp.IsSuccess)
            {
                return Result<TaskStatusResponse>.Error($"Failed to get generate. Reason: {resp.StatusCode}. {resp.DataAsText}"); 
            }
            
            var respModel = Serializer.DeserializeJson<TryOnResponse>(resp.DataAsText);

            if (respModel.Code > 0)
            {
                return Result<TaskStatusResponse>.Error($"Failed to get generate. Reason: {respModel.Code}. {respModel.Message}"); 
            }

            return Result<TaskStatusResponse>.Success(new TaskStatusResponse
            {
                TaskId = respModel.Data.TaskId,
                TaskStatus = respModel.Data.TaskStatus,
                ImageUrls = respModel.Data.TaskResult.Images?.Select(x => x.Url).ToArray()
            });
        }

        private async Task<string> GetToken(CancellationToken token = default)
        {
            if (!string.IsNullOrEmpty(_token) && (_expiresAt - DateTime.UtcNow).TotalMinutes >= 1)
            {
                return _token;
            }
            
            var url = Extensions.CombineUrls(_tokenGenerationHost, "ai/kling/token");
            var resp = await SendRequestForSingleModel<TokenResponse>(url, token);
            if (resp.IsError)
            {
                Debug.LogError("Failed to get image generation token");
                return string.Empty;
            }
            
            _token = resp.Model.Token;
            _expiresAt = resp.Model.ExpiresAt;

            return _token;
        }
    }
}