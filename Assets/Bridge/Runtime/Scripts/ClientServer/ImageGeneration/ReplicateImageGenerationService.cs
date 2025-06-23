using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Bridge.ClientServer.ImageGeneration
{
    internal sealed class ReplicateImageGenerationService: ImageGenerationServiceBase
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore, 
            ContractResolver = new LowercaseContractResolver(),
            Culture = CultureInfo.InvariantCulture,
            Converters = { new StringEnumConverter() }
        };
        
        public ReplicateImageGenerationService(string host, IRequestHelper requestHelper, ISerializer serializer, TempFileCache cache) : base(host, requestHelper, serializer, cache)
        {
        }

        public async Task<Result<ReplicateResultResponse>> GenerateImage(ReplicateRequest req, CancellationToken token)
        {
            try
            {
                return await GenerateImageInternal(req, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException 
                    ? Result<ReplicateResultResponse>.Cancelled() 
                    : Result<ReplicateResultResponse>.Error(e.Message);
            }
        }

        protected override string SerializeToJson(object obj)
        {
            return Serializer.SerializeToJson(obj, _jsonSerializerSettings);
        }

        private async Task<Result<ReplicateResultResponse>> GenerateImageInternal(ReplicateRequest req, CancellationToken token)
        {
            var initGenerationUrl = ConcatUrl(Host, "ai/v1/replicate");
            var initResult = await SendPostRequest<ReplicateProgressResponse>(initGenerationUrl, req);
            if (!initResult.IsSuccess) return Result<ReplicateResultResponse>.Error(initResult.ErrorMessage);

            var resp = await GetConvertingResult(initResult.Model.PredictionId, token);

            resp.Model.LocalFilePath = await DownloadFile(resp.Model.SignedFileUrl, resp.Model.UploadId, token);
            return resp;
        }

        private async Task<Result<ReplicateResultResponse>> GetConvertingResult(string predictionId, CancellationToken token)
        {
            const int requestIntervalMs = 500;
            const float timeOutSec = 60;
            var attemptsCount = Mathf.RoundToInt(timeOutSec * 1000 / requestIntervalMs);
            var attemptCounter = 0;

            Result<ReplicateResultResponse> resp;
            var checkResultUrl = ConcatUrl(Host, $"ai/v1/replicate/{predictionId}");
            do
            {
                attemptCounter++;
                resp = await SendRequestForSingleModel<ReplicateResultResponse>(checkResultUrl, token, useProtobuf:false);
                if (!resp.IsSuccess || !resp.Model.IsReady) await Task.Delay(requestIntervalMs, token);
            } while (attemptCounter <= attemptsCount && (!resp.IsSuccess || !resp.Model.IsReady));

            return resp;
        }

        private sealed class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }
    }
}