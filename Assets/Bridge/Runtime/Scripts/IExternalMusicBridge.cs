using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services._7Digital;
using Bridge.Services._7Digital.Models.PlaylistModels;
using Bridge.Services._7Digital.Models.TrackModels;
using UnityEngine;

namespace Bridge
{
    public interface IExternalMusicBridge: IExternalTrackSearchServiceBridge
    {
        Task<ArrayResult<ExternalTrackInfo>> SearchExternalTracksAsync(string search, int pageNumber = 1, CancellationToken cancellationToken = default);
        Task<Result<Texture2D>> DownloadExternalTrackThumbnail(string url, CancellationToken cancellationToken = default);
        Task<Result<AudioClip>> DownloadExternalTrackClip(long trackId, CancellationToken cancellationToken = default);
        Task<Result<ExternalTrackInfo>> GetTrackDetails(long trackId, CancellationToken cancellationToken = default);
        Task<TracksDetailsResult> GetBatchTrackDetails(IEnumerable<long> trackIds, CancellationToken cancellationToken = default);
        Task<ArrayResult<PlaylistInfo>> GetExternalPlaylists(string targetId, int takePrevious, int takeNext, CancellationToken cancellationToken = default);
    }
}
