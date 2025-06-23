using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityReadServices
{
    internal interface IEntityReadService<T> where T: IEntity
    {
        Task<SingleEntityResult<T>> GetAsync(long id, CancellationToken cancellationToken);
        Task<EntitiesResult<T>> GetAsync(Query<T> query, CancellationToken cancellationToken);
    }
}