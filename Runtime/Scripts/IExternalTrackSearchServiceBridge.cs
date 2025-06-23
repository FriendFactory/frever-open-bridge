using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services._7Digital.Models;

namespace Bridge
{
    public interface IExternalTrackSearchServiceBridge
    {
        Task<ArrayResult<TrackInfo>> SearchExternalTracks(string searchQuery, int takeNext = 10, int skip = 0,
            CancellationToken cancellationToken = default);
    }
}