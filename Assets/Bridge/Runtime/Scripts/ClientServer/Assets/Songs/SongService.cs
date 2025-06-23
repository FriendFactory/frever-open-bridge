using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Songs
{
    internal interface ISongService
    {
        Task<Result<SongInfo>> GetSongAsync(long id, CancellationToken token);
        Task<ArrayResult<SongInfo>> GetSongsAsync(int take, int skip, string filter = null, long? genreId = null,
            long[] songIds = null, bool commercialOnly = false, long? emotionId = null, CancellationToken token = default);
    }

    internal sealed class SongService : AssetServiceBase, ISongService
    {
        private const string END_POINT = "Song";
        
        public SongService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<Result<SongInfo>> GetSongAsync(long id, CancellationToken token)
        {
            try
            {
                var url = BuildUrl($"{END_POINT}/{id}");
                return await SendRequestForSingleModel<SongInfo>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<SongInfo>.Cancelled()
                    : Result<SongInfo>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<SongInfo>> GetSongsAsync(int take, int skip, string filter = null, long? genreId = null, long[] songIds = null, bool commercialOnly = false, long? emotionId = null, CancellationToken token = default)
        {
            try
            {
                return await GetSongsAsyncInternal(take, skip, filter, genreId, songIds, commercialOnly, emotionId, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<SongInfo>.Cancelled();
            }
        }

        private async Task<ArrayResult<SongInfo>> GetSongsAsyncInternal(int take, int skip, string filter, long? genreId, long[] songIds, bool commercialOnly, long? emotionId, CancellationToken token)
        {
            var url = BuildUrl(END_POINT);
            var body = new
            {
                Take = take,
                Skip = skip,
                Name = filter,
                GenreId = genreId,
                Ids = songIds,
                CommercialOnly = commercialOnly,
                EmotionId = emotionId
            };
            return await SendRequestForListModels<SongInfo>(url, token, body);
        }
    }
}