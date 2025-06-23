using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Level;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<SingleEntityResult<T>> GetAsync<T>(long id, CancellationToken cancellationToken)
            where T : class, IEntity
        {
            return _assetManagerServerBridge.GetAsync<T>(id, cancellationToken);
        }

        public Task<EntitiesResult<T>> GetAsync<T>(Query<T> query, CancellationToken cancellationToken)
            where T : class, IEntity
        {
            return _assetManagerServerBridge.GetAsync(query, cancellationToken);
        }

        public Task<SingleEntityResult<T>> PostAsync<T>(T target, bool cacheFiles = true, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return _assetManagerServerBridge.Post(target, cacheFiles: cacheFiles, cancellationToken);
        }
        
        public Task<SingleEntityResult<T>> UpdateAsync<T>(T target, bool updateFiles = true, bool cacheFiles = true,
            CancellationToken cancellationToken = default) where T : class, IEntity
        {
            return _assetManagerServerBridge.UpdateAsync(target, updateFiles, cacheFiles ,cancellationToken);
        }

        public Task<FileUpdateResult> UpdateFilesAsync<T>(long id, List<FileInfo> files,
            CancellationToken cancellationToken) where T : class, IEntity, IFilesAttachedEntity
        {
            return _assetManagerServerBridge.UpdateFilesAsync<T>(id, files, cancellationToken);
        }

        public Task<Result> PreUploadFiles(IEntity target, bool withChildObjects, CancellationToken cancellationToken)
        {
            return _assetManagerServerBridge.PreUploadFiles(target, withChildObjects, cancellationToken);
        }

        public Task<SingleObjectResult<T>> UpdateAsync<T>(OptimizedUpdateReqBase<T> partialUpdateRequest,
            bool updateWithFiles = true, CancellationToken cancellationToken = default) where T : class, IEntity
        {
            return _assetManagerServerBridge.UpdateAsync(partialUpdateRequest, updateWithFiles, cancellationToken);
        }

        public Task<DeleteResult<T>> DeleteAsync<T>(long id, CancellationToken cancellationToken)
            where T : class, IEntity
        {
            return _assetManagerServerBridge.DeleteAsync<T>(id, cancellationToken);
        }
    }
}