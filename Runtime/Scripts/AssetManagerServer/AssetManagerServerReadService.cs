using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer.EntityReadServices;
using Bridge.Models.Common;
using Bridge.Results;

namespace Bridge.AssetManagerServer
{
    internal sealed class AssetManagerServerReadService
    {
        private readonly EntityReadServiceProvider _entityReadServiceProvider;
        
        public AssetManagerServerReadService(EntityReadServiceProvider entityReadServiceProvider)
        {
            _entityReadServiceProvider = entityReadServiceProvider;
        }
        
        public Task<SingleEntityResult<T>> GetAsync<T>(long id, CancellationToken cancellationToken) where T : IEntity
        {
            return GetService<T>().GetAsync(id, cancellationToken);
        }

        public Task<EntitiesResult<T>> GetAsync<T>(Query<T> query, CancellationToken cancellationToken) where T : IEntity
        {
            return GetService<T>().GetAsync(query, cancellationToken);
        }

        private IEntityReadService<T> GetService<T>() where T:IEntity
        {
            return _entityReadServiceProvider.GetReadService<T>();
        }
    }
}