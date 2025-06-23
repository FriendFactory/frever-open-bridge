using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services._7Digital.Models.PlaylistModels;
using Bridge.Services._7Digital.Models.TrackModels;
using UnityEngine;

namespace Bridge.Services._7Digital
{
    public interface IMusicProviderService
    { 
        Task<ArrayResult<ExternalTrackInfo>> SearchTracksAsync(string search, int pageNumber = 1, CancellationToken cancellationToken = default);
        Task<Result<Texture2D>> DownloadTrackThumbnail(string url, CancellationToken cancellationToken = default);
        Task<Result<AudioClip>> DownloadTrackClip(long trackId, CancellationToken cancellationToken = default);
        Task<Result<ExternalTrackInfo>> GetTrack(long trackId, CancellationToken cancellationToken = default);
        Task<TracksDetailsResult> GetBatchTrackDetails(IEnumerable<long> trackIds,
            CancellationToken cancellationToken = default);
        Task<ArrayResult<PlaylistInfo>> GetExternalPlaylists(string targetId, int takePrevious, int takeNext, CancellationToken cancellationToken = default);
    }
}
