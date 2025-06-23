using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Services.ContentModeration.Models;

namespace Bridge.Services.ContentModeration
{
    internal sealed class ContentModerationService : IContentModerationService
    {
        private const string END_POINT = "moderate";
        
        private readonly string _host;
        private readonly ISerializer _serializer;
        private readonly IRequestHelper _requestHelper;

        public ContentModerationService(string host, ISerializer serializer, IRequestHelper requestHelper)
        {
            _host = host;
            _serializer = serializer;
            _requestHelper = requestHelper;
        }
        
        public async Task<ModeratedContentResult> ModerateTextContent(string text)
        {
            var url = Extensions.CombineUrls(_host, $"{END_POINT}/text");
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            request.AddField("text", text);
            
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return ModeratedContentResult.Error($"Failed to moderate text content. [Reason]: {resp.Message}");
            }
            
            var result = _serializer.DeserializeJson<ModerationResponse>(resp.DataAsText);
            return ModeratedContentResult.Success(result.PassedModeration, result.Reason);
        }
        
        public async Task<ModeratedContentResult> ModerateMediaContent(string uploadId, string fileExtension)
        {
            var url =  Extensions.CombineUrls(_host, $"{END_POINT}/uploaded-visual/{uploadId}/{fileExtension}");
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return ModeratedContentResult.Error($"Failed to moderate media content. [Reason]: {resp.Message}");
            }
            var result = _serializer.DeserializeJson<ModerationResponse>(resp.DataAsText);
            return ModeratedContentResult.Success(result.PassedModeration, result.Reason);
        }
    }
}
