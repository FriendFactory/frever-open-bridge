using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer.EntityReadServices;
using Bridge.AssetManagerServer.EntityWriteServices;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge.AssetManagerServer
{
    internal sealed class AssetManagerServerBridge : IAssetManagerServerBridge
    {
        private readonly AssetManagerServerReadService _readService;
        private readonly AssetManagerServerWriteService _writeService;

        public AssetManagerServerBridge(
            string obsoleteServerHost, IRequestHelper requestHelper,
            AssetsCache assetsCache, ISerializer serializer, ModelsFileUploader modelsFileUploader)
        {
            var cleanerProvider = new ModelCleanerProvider();
            var fileCache = new ModelsToCacheSaver(assetsCache);
            var resolverProvider = new ContractResolverProvider();
            var responseReaderProvider = new ResponseReaderProvider(serializer);
            var entitiesReadServiceProvider =
                new EntityReadServiceProvider(obsoleteServerHost, requestHelper, responseReaderProvider);
           
            _readService = new AssetManagerServerReadService(entitiesReadServiceProvider);

            var writeServicesProvider = new EntityWriteServiceProvider(obsoleteServerHost, requestHelper, responseReaderProvider,
                fileCache, modelsFileUploader, resolverProvider, serializer,
                cleanerProvider); 
            _writeService = new AssetManagerServerWriteService(writeServicesProvider);
        }

        public Task<SingleEntityResult<T>> GetAsync<T>(long id, CancellationToken cancellationToken) where T : class, IEntity
        {
            return _readService.GetAsync<T>(id, cancellationToken);
        }

        public Task<EntitiesResult<T>> GetAsync<T>(Query<T> query, CancellationToken cancellationToken) where T : class, IEntity
        {
            return _readService.GetAsync(query, cancellationToken);
        }

        public Task<SingleEntityResult<T>> Post<T>(T target, bool cacheFiles = true, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return _writeService.Post(target, cacheFiles: cacheFiles, cancellationToken);
        }

        public Task<SingleEntityResult<T>> UpdateAsync<T>(T target, bool updateFiles, bool cacheFiles, CancellationToken cancellationToken) 
            where T : class, IEntity
        {
            return _writeService.UpdateAsync(target, updateFiles, cacheFiles, cancellationToken);
        }

        public Task<FileUpdateResult> UpdateFilesAsync<T>(long id, List<FileInfo> files, CancellationToken cancellationToken) 
            where T : class, IEntity, IFilesAttachedEntity
        {
            return _writeService.UpdateFilesAsync<T>(id, files, cancellationToken);
        }

        public Task<SingleObjectResult<T>> UpdateAsync<T>(OptimizedUpdateReqBase<T> partialUpdateRequest, bool updateFiles, CancellationToken cancellationToken) 
            where T : class, IEntity
        {
            return _writeService.UpdateAsync(partialUpdateRequest, updateFiles, cancellationToken);
        }
        
        public Task<DeleteResult<T>> DeleteAsync<T>(long id, CancellationToken cancellationToken) where T : class, IEntity
        {
            return _writeService.DeleteAsync<T>(id, cancellationToken);
        }

        public Task<Result> PreUploadFiles(IEntity target, bool withChildModels, CancellationToken cancellationToken)
        {
            return _writeService.PreUploadFiles(target, withChildModels, cancellationToken);
        }
    }
}