using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services._7Digital.Models;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ArrayResult<TrackInfo>> SearchExternalTracks(string searchQuery, int takeNext = 10, int skip = 0,
            CancellationToken cancellationToken = default)
        {
            return _externalTrackSearchService.SearchExternalTracks(searchQuery, takeNext, skip, cancellationToken);
        }
    }
}