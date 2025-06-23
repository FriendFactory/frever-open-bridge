using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.AsseManager.Extensions.FilesContainable;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Newtonsoft.Json;

namespace Bridge.AssetManagerServer.EntityReadServices
{
    internal abstract class BaseEntityWriteService : BaseEntityService
    {
        private const string FILE_LOCAL_PATH_FIELD = nameof(FileInfo.FilePath);

        private static readonly string[] FILES_PROP_NAMES =
        {
            nameof(IFilesAttachedEntity.Files), nameof(FileInfo.State)
        };

        private readonly ModelCleanerProvider _cleanerProvider;

        private readonly ModelsToCacheSaver _fileCache;
        private readonly ModelsFileUploader _modelsFileUploader;
        private readonly ContractResolverProvider _resolverProvider;
        private readonly ISerializer _serializer;

        protected BaseEntityWriteService(string host, IRequestHelper requestHelper,
            ResponseReaderProvider responseReaderProvider, ContractResolverProvider resolverProvider,
            ISerializer serializer, ModelCleanerProvider cleanerProvider, ModelsToCacheSaver fileCache,
            ModelsFileUploader modelsFileUploader) : base(host, requestHelper, responseReaderProvider)
        {
            _resolverProvider = resolverProvider;
            _serializer = serializer;
            _cleanerProvider = cleanerProvider;
            _fileCache = fileCache;
            _modelsFileUploader = modelsFileUploader;
        }

        protected string CreateStringContent<T>(object sendModel, bool ignoreFiles) where T : IEntity
        {
            var resolver = _resolverProvider.GetResolver<T>();
            return Serialize(sendModel, ignoreFiles, resolver);
        }

        protected async Task<Result> UploadFiles(List<IFilesAttachedEntity> filesModels,
            CancellationToken cancellationToken)
        {
            if (filesModels.Count > 0)
                return await _modelsFileUploader.UploadFiles(filesModels.SelectMany(x => x.Files).ToArray(),
                    cancellationToken);

            return new SuccessResult();
        }

        protected Task<Result> UploadFiles(ICollection<FileInfo> fileInfos, CancellationToken cancellationToken)
        {
            return _modelsFileUploader.UploadFiles(fileInfos, cancellationToken);
        }

        protected T SyncIdAndForeignKeysAsync<T>(T source, T dest) where T : IEntity
        {
            var syncer = new ModelDataSynchronizer();
            syncer.Sync(source, dest);
            return dest;
        }

        protected async Task<SingleEntityResult<T>> SendRequestAsync<T>(
            T target, bool withFiles, HTTPMethods httpMethod, string url,
            bool syncWithTarget = true, bool cacheFiles = true, CancellationToken cancellationToken = default)
            where T : IEntity
        {
            List<IFilesAttachedEntity> filesModels = null;

            var sendingModel = GetCleanSendingModel(target);

            if (withFiles)
            {
                filesModels = GetFileContainedModels(sendingModel);
                ClearFromNotChangedFiles(filesModels);

                var uploadingRes = await UploadFiles(filesModels, cancellationToken);
                if (uploadingRes.IsError)
                    return new SingleEntityResult<T>(uploadingRes.ErrorMessage);
            }

            var jsonPart = CreateStringContent<T>(sendingModel, !withFiles);
            var req = RequestHelper.CreateRequest(url, httpMethod, true, false);
            req.AddJsonContent(jsonPart);
            var response = await req.GetHTTPResponseAsync(cancellationToken);

            if (!response.IsSuccess)
                return new SingleEntityResult<T>(response.DataAsText);
            var responseObj = ReadResult<T>(response);
            var output = syncWithTarget ? SyncIdAndForeignKeysAsync(responseObj, target) : responseObj;

            if (withFiles && filesModels != null)
            {
                SyncIdAndForeignKeysAsync(responseObj, sendingModel);
                if (cacheFiles)
                {
                    var filesDataWithUpdatedVersions = GetFileContainedModels(sendingModel);
                    await _fileCache.SaveFiles(filesDataWithUpdatedVersions);
                }
             
                UpdateFileContainableModelStates(output);
            }

            return new SingleEntityResult<T>(output);
        }

        private void UpdateFileContainableModelStates<T>(T output) where T : IEntity
        {
            var outputFileModels = GetFileContainedModels(output);
            foreach (var fileModel in outputFileModels)
            {
                var files = fileModel.Files;
                if(files == null) continue;
                
                files.SetAsSyncedWithServer();
                files.ResetFilePaths();
            }
        }

        private void ClearFromNotChangedFiles(List<IFilesAttachedEntity> downloadables)
        {
            foreach (var downloadable in downloadables)
            {
                if (downloadable.Files == null || downloadable.Files.Count == 0)
                    continue;

                downloadable.Files = downloadable.Files.Where(x => x.State != FileState.SyncedWithServer).ToList();
            }
        }

        private T GetCleanSendingModel<T>(T target) where T : IEntity
        {
            var modelCleaner = _cleanerProvider.GetCleaner<T>();
            return modelCleaner.Clean(target, true);
        }

        protected List<IFilesAttachedEntity> GetFileContainedModels(IEntity entity)
        {
            return entity.ExtractAllModelWithFiles();
        }

        protected Task SaveFilesToCache(params IFilesAttachedEntity[] models)
        {
            return _fileCache.SaveFiles(models);
        }

        private string Serialize(object target, bool ignoreFilesInfo, IgnoreFieldsContractorResolver resolver)
        {
            resolver.IgnoreProperty(FILE_LOCAL_PATH_FIELD);

            if (ignoreFilesInfo)
                resolver.IgnoreProperty(FILES_PROP_NAMES);
            return _serializer.SerializeToJson(target, new JsonSerializerSettings
            {
                ContractResolver = resolver
            });
        }
    }
}