using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer
{
    internal abstract class FilesUploadingServiceBase<TResponse, TSendModel> : AssetServiceBase
    {
        private readonly ModelsFileUploader _filesUploader;
        private readonly AssetsCache _assetsCache;
       
        protected FilesUploadingServiceBase(string host, IRequestHelper requestHelper, ISerializer serializer, ModelsFileUploader filesUploader, AssetsCache assetsCache) : base(host, requestHelper, serializer)
        {
            _filesUploader = filesUploader;
            _assetsCache = assetsCache;
        }

        protected async Task<Result<TResponse>> SendModel(TSendModel model, string endPoint)
        {
            var files = CollectFiles(model);
            var uploadFilesResp = await UploadFiles(files);
            if (!uploadFilesResp.IsSuccess)
            {
                return Result<TResponse>.Error(uploadFilesResp.ErrorMessage);
            }
            
            var url = ConcatUrl(Host, endPoint);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
            var jsonBody = Serializer.SerializeToJson(model);
            req.AddJsonContent(jsonBody);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<TResponse>.Error(resp.DataAsText);
            }

            var responseModel = Serializer.DeserializeProtobuf<TResponse>(resp.Data);
            await MoveUploadedFilesToCache(model, responseModel);
            return Result<TResponse>.Success(responseModel);
        }

        protected abstract ICollection<FileInfo> CollectFiles(TSendModel model);

        protected abstract Task MoveUploadedFilesToCache(TSendModel sendModel, TResponse response);

        protected async Task SaveFileAsync<T>(T respModel, FileInfo fileInfo) where T: IFilesAttachedEntity
        {
            if (fileInfo.State != FileState.PreUploaded && fileInfo.State != FileState.ShouldBeCopiedFromSource) return;

            var fileInfoFromResponse = respModel.Files.First(x =>
                x.FileType == fileInfo.FileType && x.Resolution.Compare(fileInfo.Resolution));
            
            fileInfo.Version = fileInfoFromResponse.Version;
            await _assetsCache.SaveToCacheAsync(respModel, fileInfo);

            fileInfo.TagAsSyncedWithServer();
            fileInfo.ReleaseAssetRawDataFromRAM();
        }

        protected async Task SaveFileAsync(long entityId, Type entityType, FileInfo fileInfo)
        {
            if (fileInfo.State != FileState.PreUploaded && fileInfo.State != FileState.ShouldBeCopiedFromSource) return;
            
            await _assetsCache.SaveToCacheAsync(entityId, entityType, fileInfo);

            fileInfo.TagAsSyncedWithServer();
            fileInfo.ReleaseAssetRawDataFromRAM();
        }
        
        protected bool NeedToSave(FileInfo fileInfo)
        {
            return fileInfo != null && (fileInfo.State == FileState.PreUploaded ||
                                        fileInfo.State == FileState.ShouldBeCopiedFromSource);
        }
        
        protected async Task<Result> UploadFiles(ICollection<FileInfo> files)
        {
            var newOrModifiedFiles = files.Where(x => x.State != FileState.SyncedWithServer).ToArray();
            return await _filesUploader.UploadFiles(newOrModifiedFiles);
        }
    }
}