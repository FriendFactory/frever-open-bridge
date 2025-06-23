using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Music
{
    internal interface IMusicService
    {
        Task<Result<SoundsResult>> GetSounds(long[] songIds, long[] userSoundIds, long[] externalSongIds,
            CancellationToken token);
    }
}