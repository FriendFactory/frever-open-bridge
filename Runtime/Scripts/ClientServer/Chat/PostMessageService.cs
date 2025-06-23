using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer.Chat
{
    internal sealed class PostMessageService: FilesUploadingServiceBase<ChatMessage, AddMessageModel>
    {
        private readonly string _endPoint;

        public PostMessageService(string host, IRequestHelper requestHelper, ISerializer serializer, ModelsFileUploader filesUploader, AssetsCache assetsCache, string endPoint) : base(host, requestHelper, serializer, filesUploader, assetsCache)
        {
            _endPoint = endPoint;
        }

        public async Task<Result> PostMessage(long chatId, AddMessageModel model)
        {
            try
            {
                return await SendModel(model, $"{_endPoint}/{chatId}/message");
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }
        
        public async Task<Result> PostMessage(SendMessageToGroupsModel model)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/message");
                return await SendPostRequest(url, model);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        protected override ICollection<FileInfo> CollectFiles(AddMessageModel model)
        {
            if (model.Files == null) return Array.Empty<FileInfo>();
            return model.Files;
        }

        protected override async Task MoveUploadedFilesToCache(AddMessageModel sendModel, ChatMessage response)
        {
            if (sendModel.Files == null) return;
            foreach (var file in sendModel.Files)
            {
                if (!NeedToSave(file)) continue;
                await SaveFileAsync(response, file);
            }
        }
    }
}