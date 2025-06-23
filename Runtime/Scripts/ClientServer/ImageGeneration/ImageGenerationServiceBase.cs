using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer.ImageGeneration
{
    internal abstract class ImageGenerationServiceBase : ServiceBase
    {
        private readonly TempFileCache _cache;
        
        protected ImageGenerationServiceBase(string host, IRequestHelper requestHelper, ISerializer serializer, TempFileCache cache) : base(host, requestHelper, serializer)
        {
            _cache = cache;
        }
        
        protected async Task<string> DownloadFile(string url, string uploadId, CancellationToken token = default)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, false, false);
            var resp = await req.GetRawDataAsync(token: token);
            return await _cache.SaveAsync($"{uploadId}.png", resp);
        }
    }
}