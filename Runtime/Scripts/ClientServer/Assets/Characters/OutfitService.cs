using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer.Assets.Characters
{
    internal interface IOutfitService
    {
        Task<Result<OutfitFullInfo>> GetOutfitAsync(long id, CancellationToken token);
        Task<ArrayResult<OutfitShortInfo>> GetOutfitListAsync(int take, int skip, SaveOutfitMethod outfitSaveType, long genderId, CancellationToken token = default);
        Task<Result<OutfitFullInfo>> SaveOutfitAsync(OutfitSaveModel saveModel);
        Task<Result> DeleteOutfit(long id);
    }
    
    internal sealed class OutfitService: FilesUploadingServiceBase<OutfitFullInfo, OutfitSaveModel>, IOutfitService
    {
        private const string OutfitEndPoint = "Outfit";
        
        public OutfitService(string host, IRequestHelper requestHelper, ISerializer serializer, ModelsFileUploader filesUploader, AssetsCache assetsCache) : base(host, requestHelper, serializer, filesUploader, assetsCache)
        {
        }

        protected override ICollection<FileInfo> CollectFiles(OutfitSaveModel model)
        {
            return model.Files;
        }

        protected override async Task MoveUploadedFilesToCache(OutfitSaveModel sendModel, OutfitFullInfo response)
        {
            foreach (var file in sendModel.Files)
            {
                if(!NeedToSave(file)) continue;
                await SaveFileAsync(response, file);
            }
        }

        public async Task<Result<OutfitFullInfo>> GetOutfitAsync(long id, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{OutfitEndPoint}/{id}");
                return await SendRequestForSingleModel<OutfitFullInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<OutfitFullInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<OutfitShortInfo>> GetOutfitListAsync(int take, int skip, SaveOutfitMethod outfitSaveType, long genderId, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{OutfitEndPoint}?take={take}&skip={skip}&saveMethod={outfitSaveType}&genderId={genderId}");
                return await SendRequestForListModels<OutfitShortInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<OutfitShortInfo>.Cancelled();
            }
        }

        public Task<Result<OutfitFullInfo>> SaveOutfitAsync(OutfitSaveModel saveModel)
        {
            return SendModel(saveModel, $"{OutfitEndPoint}");
        }

        public Task<Result> DeleteOutfit(long id)
        {
            var url = ConcatUrl(Host, $"{OutfitEndPoint}/{id}");
            return SendDeleteRequest(url);
        }
    }
}