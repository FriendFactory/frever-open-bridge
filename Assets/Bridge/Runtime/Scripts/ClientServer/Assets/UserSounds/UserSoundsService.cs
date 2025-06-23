using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Models.Common.Files;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer.Assets.UserSounds
{
    internal interface IUserSoundsService
    {
        Task<Result<UserSoundFullInfo>> GetUserSoundAsync(long id, CancellationToken token);
        Task<ArrayResult<UserSoundFullInfo>> GetUserSoundsAsync(int take, int skip, CancellationToken token);
        Task<ArrayResult<TrendingUserSound>> GetTrendingUserSoundsAsync(string searchQuery, int take, int skip,
            CancellationToken token);
        Task<Result<UserSoundFullInfo>> CreateUserSoundAsync(CreateUserSoundModel model);
        Task<Result<UserSoundFullInfo>> UpdateUserSoundNameAsync(long id, string name, CancellationToken token);
    }

    internal sealed class UserSoundsService : FilesUploadingServiceBase<UserSoundFullInfo,CreateUserSoundModel>, IUserSoundsService
    {
        private const string EndPoint = "UserSound";
        
        public UserSoundsService(string host, IRequestHelper requestHelper, ISerializer serializer, ModelsFileUploader filesUploader, AssetsCache assetsCache) :
            base(host, requestHelper, serializer, filesUploader, assetsCache)
        {
        }

        public async Task<Result<UserSoundFullInfo>> GetUserSoundAsync(long id, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{RootEndPoint}/{EndPoint}/{id}");
                return await SendRequestForSingleModel<UserSoundFullInfo>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<UserSoundFullInfo>.Cancelled()
                    : Result<UserSoundFullInfo>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<UserSoundFullInfo>> GetUserSoundsAsync(int take, int skip, CancellationToken token)
        {
            try
            {
                return await GetUserSoundsAsyncInternal(take, skip, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<UserSoundFullInfo>.Cancelled();
            }
        }
        
        public async Task<ArrayResult<TrendingUserSound>> GetTrendingUserSoundsAsync(string searchQuery, int take, int skip,
            CancellationToken token)
        {
            try
            {
                return await GetTrendingUserSoundsAsyncInternal(searchQuery, take, skip, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TrendingUserSound>.Cancelled();
            }
        }

        public Task<Result<UserSoundFullInfo>> CreateUserSoundAsync(CreateUserSoundModel model)
        {
            var endPoint = $"{RootEndPoint}/{EndPoint}";
            return SendModel(model, endPoint);
        }

        public async Task<Result<UserSoundFullInfo>> UpdateUserSoundNameAsync(long id, string name, CancellationToken token)
        {
            try
            {
                var req = RequestHelper.CreateRequest(BuildUrl($"{EndPoint}/{id}"), HTTPMethods.Patch, true, true);
                var json = Serializer.SerializeToJson(new {name = name});
                req.AddJsonContent(json);
                var resp = await req.GetHTTPResponseAsync(token: token);
                
                if (resp.IsSuccess)
                {
                    var resModel = Serializer.DeserializeProtobuf<UserSoundFullInfo>(resp.Data);
                    return Result<UserSoundFullInfo>.Success(resModel);
                }

                var error = resp.DataAsText;
                return Result<UserSoundFullInfo>.Error(error);
            }
            catch (OperationCanceledException)
            {
                return Result<UserSoundFullInfo>.Cancelled();
            }
        }

        private async Task<ArrayResult<UserSoundFullInfo>> GetUserSoundsAsyncInternal(int take, int skip, CancellationToken token)
        {
            var url = BuildUrl($"{EndPoint}/my");
            var body = new
            {
                Take = take,
                Skip = skip
            };
            return await SendRequestForListModels<UserSoundFullInfo>(url, token, body);
        }
        
        private async Task<ArrayResult<TrendingUserSound>> GetTrendingUserSoundsAsyncInternal(string searchQuery, int take, int skip, CancellationToken token)
        {
            try
            {
                var url = BuildUrl($"{EndPoint}/trending?filter={searchQuery}&take={take}&skip={skip}");
                return await SendRequestForListModels<TrendingUserSound>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ArrayResult<TrendingUserSound>.Cancelled() : ArrayResult<TrendingUserSound>.Error(e.Message);
            }
        }

        protected override ICollection<FileInfo> CollectFiles(CreateUserSoundModel model)
        {
            return model.Files;
        }

        protected override async Task MoveUploadedFilesToCache(CreateUserSoundModel sendModel, UserSoundFullInfo response)
        {
            foreach (var fileInfo in sendModel.Files)
            {
                await SaveFileAsync(response, fileInfo);
            }
        }
    }
}