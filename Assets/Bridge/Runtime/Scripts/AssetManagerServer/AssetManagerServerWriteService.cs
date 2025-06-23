using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer.EntityWriteServices;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge.AssetManagerServer
{
    internal sealed class AssetManagerServerWriteService
    {
        private readonly EntityWriteServiceProvider _serviceProvider;

        public AssetManagerServerWriteService(EntityWriteServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<SingleEntityResult<T>> Post<T>(T target, bool cacheFiles = true, CancellationToken cancellationToken = default) where T : class, IEntity
        {
            return _serviceProvider.GetEntityWriteService<T>().Post(target, cacheFiles: cacheFiles,  cancellationToken);
        }

        public Task<SingleEntityResult<T>> UpdateAsync<T>(T target, bool updateFiles, bool cacheFiles = true, CancellationToken cancellationToken = default) where T : class, IEntity
        {
            return _serviceProvider.GetEntityWriteService<T>().UpdateAsync(target, updateFiles, cacheFiles:cacheFiles, cancellationToken);
        }
        
        public Task<SingleObjectResult<T>> UpdateAsync<T>(OptimizedUpdateReqBase<T> partialUpdateRequest, bool updateFiles, CancellationToken cancellationToken) where T : class, IEntity
        {
            return _serviceProvider.GetEntityWriteService<T>()
                .UpdateAsync(partialUpdateRequest, updateFiles, cancellationToken);
        }
        
        public Task<FileUpdateResult> UpdateFilesAsync<T>(long id, List<FileInfo> files, CancellationToken cancellationToken) where T : class, IEntity, IFilesAttachedEntity
        {
            return _serviceProvider.GetFileUpdateService<T>().UpdateFilesAsync(id, files, cancellationToken);
        }
        
        public Task<DeleteResult<T>> DeleteAsync<T>(long id, CancellationToken cancellationToken) where T : class, IEntity
        {
            return _serviceProvider.GetEntityWriteService<T>().DeleteAsync(id, cancellationToken);
        }
        
        public Task<Result> PreUploadFiles<T>(T target, bool withChildModels, CancellationToken cancellationToken) where T : class, IEntity
        {
            return _serviceProvider.GetPreUploadFileService()
                .PreUploadFiles(target, withChildModels, cancellationToken);
        }
    }
}