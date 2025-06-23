using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services.Advertising;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ArrayResult<PromotedSong>> GetPromotedSongs(int take, int skip, CancellationToken token = default)
        {
            return _advertisingService.GetPromotedSongs(take, skip, token);
        }
    }
}