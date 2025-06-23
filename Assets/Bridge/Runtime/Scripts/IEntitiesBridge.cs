using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.ClientServer.Template;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge
{
    public interface IEntitiesBridge: ITemplateService
    {
        long PublicGroupId { get; }
        
        Task<SingleEntityResult<T>> GetAsync<T>(long id, CancellationToken cancellationToken = default) where T :class, IEntity;
        Task<EntitiesResult<T>> GetAsync<T>(Query<T> query, CancellationToken cancellationToken = default) where T : class, IEntity;
        Task<SingleEntityResult<T>> PostAsync<T>(T target, bool cacheFiles = true, CancellationToken cancellationToken = default) where T : class, IEntity;
        Task<SingleEntityResult<T>> UpdateAsync<T>(T target, bool updateFiles = true, bool cacheFiles = true, CancellationToken cancellationToken = default) where T : class, IEntity;
        Task<FileUpdateResult> UpdateFilesAsync<T>(long id, List<FileInfo> files, CancellationToken cancellationToken = default) where T : class, IEntity, IFilesAttachedEntity;
        Task<Result> PreUploadFiles(IEntity target, bool withChildObjects, CancellationToken cancellationToken = default);
        Task<SingleObjectResult<T>> UpdateAsync<T>(OptimizedUpdateReqBase<T> partialUpdateRequest, bool updateWithFiles = true, CancellationToken cancellationToken = default) where T : class, IEntity;
        Task<DeleteResult<T>> DeleteAsync<T>(long id, CancellationToken cancellationToken = default) where T : class, IEntity;
    }
}