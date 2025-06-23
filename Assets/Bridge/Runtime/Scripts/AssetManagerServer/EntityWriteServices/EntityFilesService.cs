using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer.EntityReadServices;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using UnityEngine;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    internal sealed class EntityFilesService<T> : BaseEntityWriteService, IEntityFilesService<T>
        where T : class, IEntity, IFilesAttachedEntity
    {
        public EntityFilesService(string host, IRequestHelper requestHelper,
            ResponseReaderProvider responseReaderProvider, ContractResolverProvider resolverProvider,
            ISerializer serializer, ModelCleanerProvider cleanerProvider, ModelsToCacheSaver fileCache, ModelsFileUploader modelsFileUploader) :
            base(host, requestHelper, responseReaderProvider, resolverProvider, serializer,
                cleanerProvider, fileCache, modelsFileUploader)
        {
        }

        public async Task<FileUpdateResult> UpdateFilesAsync(long id, List<FileInfo> files,
            CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => PatchFilesAsyncInternal(id, files, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledFileUpdateResult();
            }
        }

        private async Task<FileUpdateResult> PatchFilesAsyncInternal(long id,
            List<FileInfo> modelFiles, CancellationToken cancellationToken)
        {
            if (modelFiles == null || modelFiles.Count == 0 ||
                modelFiles.All(x => x.State == FileState.SyncedWithServer))
                return new FailUpdateResult(
                    $"You must attach at least 1 modified file. You have tried to send {modelFiles?.Count} files. State of files: {modelFiles?.FirstOrDefault()?.State}");

            var url = GetBaseUrl<T>();

            var uploadingFilesResult = await UploadFiles(modelFiles, cancellationToken);
            if (uploadingFilesResult.IsError)
                return new FailUpdateResult(uploadingFilesResult.ErrorMessage);

            var req = RequestHelper.CreateRequest(url, HTTPMethods.Patch, true, false);
            var sendModel = new
            {
                Id = id,
                Files = modelFiles
            };
            var json = CreateStringContent<T>(sendModel, false);
            req.AddJsonContent(json);

            var res = await req.GetHTTPResponseAsync(cancellationToken);
            var result = GetFileUpdateResultAsync(res);
            if (result.IsSuccess)
            {
                var responseModel = ReadResult<T>(res);
                SyncFileVersions(responseModel.Files, modelFiles);

                //todo: adapt models to prevent creating new fake instance
                var instance = Activator.CreateInstance<T>();
                instance.Id = id;
                instance.Files = modelFiles;

                await SaveFilesToCache(instance);
            }

            return result;
        }

        private void SyncFileVersions(List<FileInfo> source, List<FileInfo> dest)
        {
            foreach (var destFileInfo in dest)
            {
                var sourceFileInfo = source.FirstOrDefault(x =>
                    x.FileType == destFileInfo.FileType &&
                    x.Resolution == destFileInfo.Resolution);
                if (sourceFileInfo == null)
                {
                    Debug.LogError("There is source file info for destination file info. Version can't be synced");
                    continue;
                }

                destFileInfo.Version = sourceFileInfo.Version;
            }
        }

        private FileUpdateResult GetFileUpdateResultAsync(HTTPResponse res)
        {
            if (res.IsSuccess) return new SuccessUpdateResult();

            var error = res.DataAsText;
            return new FailUpdateResult(error);
        }
    }
}