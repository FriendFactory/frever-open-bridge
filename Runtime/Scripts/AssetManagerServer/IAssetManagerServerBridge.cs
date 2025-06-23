using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge.AssetManagerServer
{
    internal interface IAssetManagerServerBridge
    {
        Task<SingleEntityResult<T>> GetAsync<T>(long id, CancellationToken cancellationToken)
            where T : class, IEntity;

        Task<EntitiesResult<T>> GetAsync<T>(Query<T> query, CancellationToken cancellationToken)
            where T : class, IEntity;

        Task<SingleEntityResult<T>> Post<T>(T target, bool cacheFiles = true, CancellationToken cancellationToken = default)
            where T : class, IEntity;

        Task<SingleEntityResult<T>> UpdateAsync<T>(T target, bool updateFiles, bool cacheFiles = true, CancellationToken cancellationToken = default)
            where T : class, IEntity;

        Task<FileUpdateResult> UpdateFilesAsync<T>(long id, List<FileInfo> files, CancellationToken cancellationToken)
            where T : class, IEntity, IFilesAttachedEntity;

        Task<SingleObjectResult<T>> UpdateAsync<T>(OptimizedUpdateReqBase<T> partialUpdateRequest, bool updateFiles,
            CancellationToken cancellationToken) where T : class, IEntity;

        Task<DeleteResult<T>> DeleteAsync<T>(long id, CancellationToken cancellationToken)
            where T : class, IEntity;

        Task<Result> PreUploadFiles(IEntity target, bool withChildModels, CancellationToken cancellationToken);
    }
}