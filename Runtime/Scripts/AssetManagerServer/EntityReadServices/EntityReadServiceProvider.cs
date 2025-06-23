using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer.EntityReadServices
{
    internal sealed class EntityReadServiceProvider
    {
        private readonly string _obsoleteServerHost;
        private readonly IRequestHelper _requestHelper;
        private readonly ResponseReaderProvider _responseReaderProvider;
        
        public EntityReadServiceProvider(string obsoleteServerHost, IRequestHelper requestHelper,
            ResponseReaderProvider responseReaderProvider)
        {
            _obsoleteServerHost = obsoleteServerHost;
            _requestHelper = requestHelper;
            _responseReaderProvider = responseReaderProvider;
        }
        
        public IEntityReadService<T> GetReadService<T>() where T : IEntity
        {
            return new DefaultEntityReadService<T>(_obsoleteServerHost, _requestHelper, _responseReaderProvider);
        }
    }
}