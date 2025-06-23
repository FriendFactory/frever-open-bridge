using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Music
{
    public interface IFavouriteMusicService
    {
        Task<ArrayResult<FavouriteMusicInfo>> GetFavouriteSoundList(int take, int skip, bool commercialOnly, CancellationToken token);
        Task<Result<FavouriteMusicInfo>> AddSoundToFavouriteList(SoundType soundType, long id);
        Task<Result> RemoveSoundFromFavouriteList(SoundType soundType, long id);
    }
    
    internal sealed class FavouriteMusicService: AssetServiceBase, IFavouriteMusicService
    {
        private const string END_POINT = "favorite-sound";

        public FavouriteMusicService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<FavouriteMusicInfo>> GetFavouriteSoundList(int take, int skip, bool commercialOnly, CancellationToken token)
        {
            try
            {
                var url = BuildUrl($"{END_POINT}?take={take}&skip={skip}&commercialOnly={commercialOnly}");
                return await SendRequestForListModels<FavouriteMusicInfo>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<FavouriteMusicInfo>.Cancelled()
                    : ArrayResult<FavouriteMusicInfo>.Error(e.Message);
            }
        }

        public async Task<Result<FavouriteMusicInfo>> AddSoundToFavouriteList(SoundType soundType, long id)
        {
            var url = GetModifyFavouriteListUrl(soundType, id);
            return await SendPostRequest<FavouriteMusicInfo>(url);
        }

        public async Task<Result> RemoveSoundFromFavouriteList(SoundType soundType, long id)
        {
            try
            {
                return await SendDeleteRequest(GetModifyFavouriteListUrl(soundType, id));
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        private string GetModifyFavouriteListUrl(SoundType soundType, long id)
        {
            return BuildUrl($"{END_POINT}/{id}/{(int)soundType}");
        }
    }
}