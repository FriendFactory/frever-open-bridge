using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bridge.ClientServer.ImageGeneration
{
    internal sealed class StabilityImageGenerationService : ImageGenerationServiceBase
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore, 
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CustomNamingStrategy()
            },
            Culture = CultureInfo.InvariantCulture
        };

        private readonly TempFileCache _cache;

        public StabilityImageGenerationService(string host, IRequestHelper requestHelper, ISerializer serializer, TempFileCache tempFileCache) : base(host, requestHelper, serializer, tempFileCache)
        {
            _cache = tempFileCache;
        }

        public async Task<Result<StabilityCreateImageResponse>> GenerateImage(CreateImageRequest model)
        {
            try
            {
                return await GenerateImageInternal(model);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return Result<StabilityCreateImageResponse>.Cancelled();
                return Result<StabilityCreateImageResponse>.Error(e.Message);
            }
        }

        protected override string SerializeToJson(object obj)
        {
            return Serializer.SerializeToJson(obj, _jsonSerializerSettings);
        }
        
        private async Task<Result<StabilityCreateImageResponse>> GenerateImageInternal(CreateImageRequest model)
        {
            var url = ConcatUrl(Host, "ai/text-to-image/stable-diffusion-xl-1024-v1-0");
            var result = await SendPostRequest<StabilityCreateImageResponse>(url, model);
            if (!result.IsSuccess) return result;
            result.Model.LocalFilePath = await DownloadFile(result.Model.SignedFileUrl, result.Model.UploadId);
            return result;
        }
    }
    
    internal sealed class CustomNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            var result = Regex.Replace(name, "([A-Z])", m => (m.Index > 0 ? "_" : "") + m.Value[0].ToString().ToLowerInvariant());
            return result;
        }
    }
}