using BestHTTP;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityReadServices
{
    internal abstract class BaseEntityService
    {
        protected readonly string ObseleteServerHost;
        protected readonly IRequestHelper RequestHelper;
        protected readonly ResponseReaderProvider ResponseReaderProvider;

        protected BaseEntityService(string host, IRequestHelper requestHelper, ResponseReaderProvider responseReaderProvider)
        {
            ObseleteServerHost = host;
            RequestHelper = requestHelper;
            ResponseReaderProvider = responseReaderProvider;
        }
        
        protected string GetBaseUrl<T>() where T : IEntity
        {
            return $"{ObseleteServerHost}{typeof(T).Name}/".FixUrlSlashes();
        }

        protected T ReadResult<T>(HTTPResponse resp)
        {
            return ResponseReaderProvider.GetResponseReader(resp).ReadObject<T>(resp);
        }
        
        protected EntitiesResult<T> GetArrayResultAsync<T>(HTTPResponse res) where T : IEntity
        {
            if (res.IsSuccess)
            {
                var reader = ResponseReaderProvider.GetResponseReader(res);
                var models = reader.ReadArray<T>(res);
                return new EntitiesResult<T>(models);
            }

            var error = res.DataAsText;
            return new EntitiesResult<T>(error);
        }

        protected string ConcatUrl(string baseurl, string append)
        {
            return Bridge.Extensions.CombineUrls(baseurl, append);
        }
    }
}