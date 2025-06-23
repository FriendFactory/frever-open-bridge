using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Music
{
    internal sealed class MusicService: AssetServiceBase, IMusicService
    {
        public MusicService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<Result<SoundsResult>> GetSounds(long[] songIds, long[] userSoundIds, long[] externalSongIds,
            CancellationToken token)
        {
            try
            {
                var url = BuildUrl("Sounds");
                var body = new
                {
                    SongIds = songIds,
                    UserSoundIds = userSoundIds,
                    ExternalSongIds = externalSongIds,
                };

                return await SendRequestForSingleModel<SoundsResult>(url, token, body: body);
            }
            catch (OperationCanceledException)
            {
                return Result<SoundsResult>.Cancelled();
            }
        }
    }
}