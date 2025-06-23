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
    public sealed partial class ServerBridge
    {
        public Task<ArrayResult<ExternalTrackInfo>> SearchExternalTracksAsync(string search, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            return _musicProviderService.SearchTracksAsync(search, pageNumber, cancellationToken);
        }

        public Task<Result<Texture2D>> DownloadExternalTrackThumbnail(string url, CancellationToken cancellationToken = default)
        {
            return _musicProviderService.DownloadTrackThumbnail(url, cancellationToken);
        }

        public Task<Result<AudioClip>> DownloadExternalTrackClip(long trackId, CancellationToken cancellationToken = default)
        {
            return _musicProviderService.DownloadTrackClip(trackId, cancellationToken);
        }

        public Task<Result<ExternalTrackInfo>> GetTrackDetails(long trackId, CancellationToken cancellationToken = default)
        {
            return _musicProviderService.GetTrack(trackId, cancellationToken);
        }

        public Task<TracksDetailsResult> GetBatchTrackDetails(IEnumerable<long> trackIds,
            CancellationToken cancellationToken = default)
        {
            return _musicProviderService.GetBatchTrackDetails(trackIds, cancellationToken);
        }

        public Task<ArrayResult<PlaylistInfo>> GetExternalPlaylists(string targetId, int takePrevious, int takeNext, CancellationToken cancellationToken = default)
        {
            return _musicProviderService.GetExternalPlaylists(targetId, takePrevious, takeNext, cancellationToken);
        }
    }
}