using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Modules.Serialization;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    internal sealed class EntityWriteServiceProvider
    {
        private readonly string _host;
        private readonly IRequestHelper _requestHelper;
        private readonly ResponseReaderProvider _responseReaderProvider;
        private readonly ModelsToCacheSaver _fileCache;
        private readonly ModelsFileUploader _modelsFileUploader;
        private readonly ContractResolverProvider _resolverProvider;
        private readonly ISerializer _serializer;
        private readonly ModelCleanerProvider _cleanerProvider;

        public EntityWriteServiceProvider(string host, IRequestHelper requestHelper, ResponseReaderProvider responseReaderProvider, ModelsToCacheSaver fileCache, ModelsFileUploader modelsFileUploader, ContractResolverProvider resolverProvider, ISerializer serializer, ModelCleanerProvider cleanerProvider)
        {
            _host = host;
            _requestHelper = requestHelper;
            _responseReaderProvider = responseReaderProvider;
            _fileCache = fileCache;
            _modelsFileUploader = modelsFileUploader;
            _resolverProvider = resolverProvider;
            _serializer = serializer;
            _cleanerProvider = cleanerProvider;
        }

        public IEntityWriteService<T> GetEntityWriteService<T>() where T : class, IEntity
        {
            return new DefaultWriteEntityService<T>(_host, _requestHelper, _responseReaderProvider, _resolverProvider,
                _serializer, _cleanerProvider, _fileCache, _modelsFileUploader);
        }

        public IEntityFilesService<T> GetFileUpdateService<T>() where T : class, IEntity, IFilesAttachedEntity
        {
            return new EntityFilesService<T>(_host, _requestHelper, _responseReaderProvider, _resolverProvider,
                _serializer, _cleanerProvider, _fileCache, _modelsFileUploader);
        }

        public IFilePreUploadService GetPreUploadFileService()
        {
            return new FilePreUploadService(_host, _requestHelper, _responseReaderProvider, _resolverProvider,
                _serializer, _cleanerProvider, _fileCache, _modelsFileUploader);
        }
    }
}