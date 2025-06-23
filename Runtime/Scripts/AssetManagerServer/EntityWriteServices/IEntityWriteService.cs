using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    /// <summary>
    /// Provides base functionality(CRUD) for entities
    /// </summary>
    internal interface IEntityWriteService<T> where T : class, IEntity
    {
        Task<SingleEntityResult<T>> Post(T target,  bool cacheFiles = true, CancellationToken cancellationToken = default);
        Task<SingleEntityResult<T>> UpdateAsync(T target, bool updateFiles, bool cacheFiles = true, CancellationToken cancellationToken = default);
        Task<SingleObjectResult<T>> UpdateAsync(OptimizedUpdateReqBase<T> partialUpdateRequest, bool updateFiles, CancellationToken cancellationToken);
        Task<DeleteResult<T>> DeleteAsync(long id, CancellationToken cancellationToken);
    }
}