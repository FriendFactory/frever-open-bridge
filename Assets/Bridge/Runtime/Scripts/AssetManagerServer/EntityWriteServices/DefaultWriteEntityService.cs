using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer.EntityReadServices;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Modules.Serialization;
using Bridge.Results;
using UnityEngine;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    internal sealed class DefaultWriteEntityService<T> : BaseEntityWriteService, IEntityWriteService<T>
        where T : class, IEntity
    {

        public DefaultWriteEntityService(string host, IRequestHelper requestHelper,
            ResponseReaderProvider responseReaderProvider, ContractResolverProvider resolverProvider,
            ISerializer serializer, ModelCleanerProvider cleanerProvider, ModelsToCacheSaver fileCache, ModelsFileUploader modelsFileUploader) :
            base(host, requestHelper, responseReaderProvider, resolverProvider, serializer,
                cleanerProvider, fileCache, modelsFileUploader)
        {
            
        }

        public async Task<SingleEntityResult<T>> Post(T target, bool cacheFiles = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return await Task.Run(() =>
                    SendRequestAsync(target, true, HTTPMethods.Post, GetBaseUrl<T>(), cacheFiles: cacheFiles,
                        cancellationToken: cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledSingleEntityResult<T>();
            }
        }

        public Task<SingleEntityResult<T>> UpdateAsync(T target, bool updateFiles, bool cacheFiles = true, CancellationToken cancellationToken = default)
        {
            return Task.Run(
                () => SendRequestAsync(target, updateFiles, HTTPMethods.Patch, GetBaseUrl<T>(), cacheFiles: cacheFiles,
                    cancellationToken: cancellationToken), cancellationToken);
        }

        public async Task<SingleObjectResult<T>> UpdateAsync(OptimizedUpdateReqBase<T> partialUpdateRequest,
            bool updateFiles, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => UpdateAsyncInternal(partialUpdateRequest, updateFiles, cancellationToken),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledSingleObjectResult<T>();
            }
        }

        public async Task<DeleteResult<T>> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => DeleteAsyncInternal(id, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledDeleteResult<T>(id);
            }
        }

        private async Task<DeleteResult<T>> DeleteAsyncInternal(long id, CancellationToken cancellationToken)
        {
            var url = GetBaseUrl<T>() + id;
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            using (var res = await req.GetHTTPResponseAsync(cancellationToken))
            {
                if (res.IsSuccess) return new DeleteResult<T>(id);

                var error = res.DataAsText;
                return new DeleteResult<T>(error);
            }
        }

        private async Task<SingleObjectResult<T>> UpdateAsyncInternal(OptimizedUpdateReqBase<T> partialUpdateRequest,
            bool updateFiles, CancellationToken cancellationToken)
        {
            List<IFilesAttachedEntity> filesModels = null;
            if (updateFiles)
            {
                filesModels = GetFileContainedModels(partialUpdateRequest.TargetModel);
                var uploadingRes = await UploadFiles(filesModels, cancellationToken);
                if (uploadingRes.IsError)
                    return new SingleEntityResult<T>(uploadingRes.ErrorMessage);
            }

            var url = GetBaseUrl<T>();
            partialUpdateRequest.BuildQueryObject(updateFiles);
            if (!partialUpdateRequest.HasDataToUpdate)
            {
                Debug.LogWarning(
                    $"There were nothing to update in patch request {partialUpdateRequest.GetType().Name}");
                return new SingleObjectResult<T>(partialUpdateRequest.OriginModel);
            }

            var queryEntity = partialUpdateRequest.QueryObject;
            
            using (var req = RequestHelper.CreateRequest(url, HTTPMethods.Patch, true, false))
            {
                var jsonContent = CreateStringContent<T>(queryEntity, updateFiles);
                req.AddJsonContent(jsonContent);

                using (var resp = await req.GetHTTPResponseAsync(cancellationToken))
                {
                    if (!resp.IsSuccess)
                        return new SingleObjectResult<T>(resp.DataAsText);

                    var resultModel = ReadResult<T>(resp);
                    SyncIdAndForeignKeysAsync(resultModel, partialUpdateRequest.TargetModel);
                    if (updateFiles && filesModels != null) await SaveFilesToCache(filesModels.ToArray());

                    return new SingleObjectResult<T>(resultModel);
                }
            }
        }
    }
}