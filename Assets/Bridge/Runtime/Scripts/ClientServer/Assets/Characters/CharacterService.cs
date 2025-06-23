using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer.Assets.Characters
{
    internal interface ICharacterService
    {
        Task<ArrayResult<CharacterInfo>> GetFriendsMainCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterInfo>> GetMyCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterInfo>> GetStarCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterInfo>> GetStyleCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterFullInfo>> GetCharacters(long[] ids, CancellationToken token = default);
        Task<Result<CharacterFullInfo>> GetCharacter(long id, CancellationToken token = default);
        Task<Result<CharacterFullInfo>> SaveCharacter(CharacterSaveModel character);
        Task<Result> UpdateCharacterName(long characterId, string name);
        Task<Result<CharacterFullInfo>> UpdateCharacterThumbnails(long characterId, params FileInfo[] thumbnails);
        Task<Result> DeleteCharacter(long id);
    }
    
    internal sealed class CharacterService: FilesUploadingServiceBase<CharacterFullInfo, CharacterSaveModel>, ICharacterService
    {
        private const string CharacterEndPoint = "character";
        
        public CharacterService(string host, IRequestHelper requestHelper, ISerializer serializer, ModelsFileUploader filesUploader, AssetsCache assetsCache) 
            : base(host, requestHelper, serializer, filesUploader, assetsCache)
        {
        }

        public Task<ArrayResult<CharacterInfo>> GetFriendsMainCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter, CancellationToken token)
        {
            return GetCharacterListAsyncInternal("friends", target, takeNext, takePrevious, filter, universeId, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetMyCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter, CancellationToken token)
        {
            return GetCharacterListAsyncInternal("my", target, takeNext, takePrevious, filter, universeId, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetStarCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter, CancellationToken token)
        {
            return GetCharacterListAsyncInternal("stars", target, takeNext, takePrevious, filter, universeId, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetStyleCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter, CancellationToken token = default)
        {
            return GetCharacterListAsyncInternal("style", target, takeNext, takePrevious, filter, universeId, token);
        }

        public async Task<ArrayResult<CharacterFullInfo>> GetCharacters(long[] ids, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{CharacterEndPoint}/Characters");
                var body = new {Ids = ids};
                return await SendRequestForListModels<CharacterFullInfo>(url, token, body);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<CharacterFullInfo>.Cancelled();
            }
        }

        public async Task<Result<CharacterFullInfo>> GetCharacter(long id, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{CharacterEndPoint}/{id}");
                return await SendRequestForSingleModel<CharacterFullInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<CharacterFullInfo>.Cancelled();
            }
        }

        public Task<Result<CharacterFullInfo>> SaveCharacter(CharacterSaveModel character)
        {
            return SendCharacterModel(character);
        }

        public async Task<Result> UpdateCharacterName(long characterId, string name)
        {
            var url = ConcatUrl(Host, $"{CharacterEndPoint}/{characterId}/name/{name}");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
            {
                return new SuccessResult();
            }
            return new ErrorResult(resp.DataAsText, resp.StatusCode);
        }

        public async Task<Result<CharacterFullInfo>> UpdateCharacterThumbnails(long characterId, params FileInfo[] thumbnails)
        {
            var uploadFilesResp = await UploadFiles(thumbnails);
            if (!uploadFilesResp.IsSuccess)
            {
                return Result<CharacterFullInfo>.Error(uploadFilesResp.ErrorMessage);
            }
            
            var url = ConcatUrl(Host, $"{CharacterEndPoint}/files");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
            var bodyJson = Serializer.SerializeToJson(new
            {
                Id = characterId,
                Files = thumbnails
            });
            req.AddJsonContent(bodyJson);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<CharacterFullInfo>.Error(resp.DataAsText);
            }

            var character = Serializer.DeserializeProtobuf<CharacterFullInfo>(resp.Data);
            foreach (var thumbnail in thumbnails)
            {
                await SaveFileAsync(character, thumbnail);
            }
            return Result<CharacterFullInfo>.Success(character);
        }

        public Task<Result> DeleteCharacter(long id)
        {
            var url = ConcatUrl(Host, $"{CharacterEndPoint}/{id}");
            return SendDeleteRequest(url);
        }

        private async Task<ArrayResult<CharacterInfo>> GetCharacterListAsyncInternal(string endPoint, long? target, int takeNext, int takePrevious, string filter, long universeId, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{CharacterEndPoint}/{endPoint}?takeNext={takeNext}&takePrevious={takePrevious}&universeId={universeId}&filter={filter}");
                if (target.HasValue)
                {
                    url += $"&target={target}";
                }
                return await SendRequestForListModels<CharacterInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<CharacterInfo>.Cancelled();
            }
        }

        private Task<Result<CharacterFullInfo>> SendCharacterModel(CharacterSaveModel character)
        {
            return SendModel(character, CharacterEndPoint);
        }

        protected override ICollection<FileInfo> CollectFiles(CharacterSaveModel model)
        {
            return model.Files;
        }

        protected override async Task MoveUploadedFilesToCache(CharacterSaveModel sendModel, CharacterFullInfo response)
        {
            foreach (var file in sendModel.Files)
            {
                if (!NeedToSave(file)) continue;
                await SaveFileAsync(response, file);
            }
        }
    }
}
