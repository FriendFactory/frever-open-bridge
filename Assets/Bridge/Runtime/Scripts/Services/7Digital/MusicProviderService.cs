using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services._7Digital.Models;
using Bridge.Services._7Digital.Models.PlaylistModels;
using Bridge.Services._7Digital.Models.TrackModels;
using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services._7Digital
{
    internal sealed class MusicProviderService : IMusicProviderService
    {
        private const float PREVIEW_CLIP_DURATION = 30000f;
        private const string USAGE_TYPE = "adsupportedstreaming";

        private readonly string _url;
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;

        public MusicProviderService(string url, IRequestHelper requestHelper, ISerializer serializer)
        {
            _url = url;
            _requestHelper = requestHelper;
            _serializer = serializer;
        }

        public async Task<ArrayResult<ExternalTrackInfo>> SearchTracksAsync(string search, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(search))
            {
                return ArrayResult<ExternalTrackInfo>.Success(Array.Empty<ExternalTrackInfo>());
            }

            var requestParameters = new Dictionary<string, string>
            {
                { "usageTypes", USAGE_TYPE },
                { "imageSize", "100" },
                { "page", $"{pageNumber}" },
                { "q", $"{search}&" }
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var baseUrl = "https://api.7digital.com/1.2/track/search";
            try
            {
                var urlResponse = await GetUrl(baseUrl, HTTPMethods.Get, requestParameters, cancellationToken);

                var res = await client.GetAsync(urlResponse.Url, cancellationToken);
                if (!res.IsSuccessStatusCode)
                {
                    return ArrayResult<ExternalTrackInfo>.Error(res.ReasonPhrase);
                }

                var content = await res.Content.ReadAsStringAsync();
                var response = _serializer.DeserializeJson<TrackSearchResponse>(content);
                if (response?.status != "error")
                {
                    var result = GetSearchResult(response);
                    return ArrayResult<ExternalTrackInfo>.Success(result);
                }

                var errorResponse = _serializer.DeserializeJson<ErrorResponse>(content);
                return ArrayResult<ExternalTrackInfo>.Error(
                    $"Searching for music tracks failed [Reason]: {errorResponse?.Error.message}");
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<ExternalTrackInfo>.Cancelled();
            }
        }

        public async Task<Result<Texture2D>> DownloadTrackThumbnail(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = _requestHelper.CreateRequest(url, HTTPMethods.Get, false, false);
                var result = await request.GetHTTPResponseAsync(cancellationToken);
                if (result.IsSuccess)
                {
                    return Result<Texture2D>.Success(result.DataAsTexture2D);
                }
                return Result<Texture2D>.Error(result.Message);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException? Result<Texture2D>.Cancelled() : Result<Texture2D>.Error(e.Message);
            }
        }

        public async Task<Result<AudioClip>> DownloadTrackClip(long trackId, CancellationToken cancellationToken = default)
        {
            try
            {
                var baseUrl = $"https://previews.7digital.com/clip/{trackId}";
                var urlResult = await GetUrl(baseUrl, HTTPMethods.Get, cancellationToken: cancellationToken);
                var result = await DownloadClipInternal(urlResult.Url, urlResult.AuthorizationHeader, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return Result<AudioClip>.Success(result.Model);
            }
            catch (OperationCanceledException)
            {
                return Result<AudioClip>.Cancelled();
            }
        }

        public async Task<Result<ExternalTrackInfo>> GetTrack(long trackId, CancellationToken cancellationToken = default)
        {
            var requestParameters = new Dictionary<string, string>
            {
                { "trackId", $"{trackId}" },
                { "usageTypes", USAGE_TYPE },
                { "imageSize", "100" }
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            var baseUrl = "https://api.7digital.com/1.2/track/details";

            try
            {
                var urlResponse = await GetUrl(baseUrl, HTTPMethods.Get, requestParameters, cancellationToken);

                if (urlResponse.IsError)
                {
                    return Result<ExternalTrackInfo>.Error(urlResponse.ErrorMessage);
                }
                
                var res = await client.GetAsync(urlResponse.Url, cancellationToken);
                var content = await res.Content.ReadAsStringAsync();
                if (!res.IsSuccessStatusCode)
                {
                    return Result<ExternalTrackInfo>.Error(res.ReasonPhrase);
                }

                var trackDetail = _serializer.DeserializeJson<TrackDetail>(content);
                var track = trackDetail.Track;
                var trackInfo = MapTrack(track);

                return Result<ExternalTrackInfo>.Success(trackInfo);
            }
            catch (OperationCanceledException)
            {
                return Result<ExternalTrackInfo>.Cancelled();
            }
        }

        public async Task<TracksDetailsResult> GetBatchTrackDetails(IEnumerable<long> trackIds,
            CancellationToken cancellationToken = default)
        {
            var requestParameters = new Dictionary<string, string>
            {
                { "trackIds", $"{string.Join(",", trackIds)}" },
                {"showErrors", "true" },
                { "usageTypes", USAGE_TYPE },
                { "imageSize", "100" }
            };
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            var baseUrl = "https://api.7digital.com/1.2/track/details/batch";
            
            try
            {
                var urlResponse = await GetUrl(baseUrl, HTTPMethods.Get, requestParameters, cancellationToken);

                var res = await client.GetAsync(urlResponse.Url, cancellationToken);
                var content = await res.Content.ReadAsStringAsync();
                if (!res.IsSuccessStatusCode)
                {
                    return TracksDetailsResult.Error(res.ReasonPhrase);
                }

                var tracksDetails = _serializer.DeserializeJson<BatchTracksDetails>(content);
                var trackInfos = tracksDetails.Items.Tracks.Select(MapTrack).ToArray();

                return TracksDetailsResult.Success(trackInfos, tracksDetails.Items.Errors);
            }
            catch (OperationCanceledException)
            {
                return TracksDetailsResult.Cancelled();
            }
        }

        public async Task<ArrayResult<PlaylistInfo>> GetExternalPlaylists(string targetId, int take, int skip, CancellationToken cancellationToken = default)
        {
            var playlistIdsResult = await GetExternalPlaylistIds(targetId, take, skip, cancellationToken);

            if (playlistIdsResult.IsError)
            {
                return ArrayResult<PlaylistInfo>.Error(playlistIdsResult.ErrorMessage);
            }

            if (playlistIdsResult.IsRequestCanceled)
            {
                return ArrayResult<PlaylistInfo>.Cancelled();
            }
            
            var playListsResult = playlistIdsResult.Models.Select(ids => GetPlaylistInfo(ids, cancellationToken)).ToArray();
            var result = await Task.WhenAll(playListsResult);

            if (result.Any(x => x.IsError))
            {
                return ArrayResult<PlaylistInfo>.Error(playlistIdsResult.ErrorMessage);
            }

            if (result.Any(x => x.IsRequestCanceled))
            {
                return ArrayResult<PlaylistInfo>.Cancelled();
            }

            var playLists = result.Select(x => x.Model).ToArray();
            return ArrayResult<PlaylistInfo>.Success(playLists);
        }

        private async Task<ArrayResult<ExternalTrackInfo>> GetTracks(IReadOnlyList<string> trackIds, CancellationToken cancellationToken = default)
        {
            var tracksString = string.Empty;
            for (var i = 0; i < trackIds.Count; i++)
            {
                tracksString += trackIds[i];
                if (i != trackIds.Count - 1) tracksString += ",";
            }

            var requestParameters = new Dictionary<string, string>
            {
                { "trackIds", $"{tracksString}" },
                { "usageTypes", USAGE_TYPE },
                { "imageSize", "100" }
            };

            try
            {
                using (var client = _requestHelper.CreateClient(false))
                {
                    client.DefaultRequestHeaders.Add("accept", "application/json");
                    var baseUrl = "https://api.7digital.com/1.2/track/details/batch";
                
                    var urlResponse = await GetUrl(baseUrl, HTTPMethods.Get, requestParameters, cancellationToken);
                    client.DefaultRequestHeaders.Authorization = CreateAuthorizationHeader(urlResponse.AuthorizationHeader);
                    var res = await client.GetAsync(urlResponse.Url, cancellationToken);
                    var content = await res.Content.ReadAsStringAsync();

                    if (!res.IsSuccessStatusCode)
                    {
                        return ArrayResult<ExternalTrackInfo>.Error(res.ReasonPhrase);
                    }

                    var trackDetails = _serializer.DeserializeJson<TracksDetails>(content);
                    var tracks = trackDetails.Items.Tracks;

                    var trackInfos = tracks.Select(MapTrack).ToArray();

                    return ArrayResult<ExternalTrackInfo>.Success(trackInfos);
                }
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<ExternalTrackInfo>.Cancelled();
            }
        }

        private async Task<Result<AudioClip>> DownloadClipInternal(string url, string header, CancellationToken cancellationToken)
        {
            var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            request.SetRequestHeader("Authorization", $"Bearer {header}");

            try
            {
                var result = await GetAsyncInternal(request, cancellationToken);
                return Result<AudioClip>.Success(DownloadHandlerAudioClip.GetContent(result));
            }
            catch (OperationCanceledException)
            {
                return Result<AudioClip>.Cancelled();
            }
            catch (Exception ex)
            {
                return Result<AudioClip>.Error(ex.Message);
            }
        }

        private async Task<string[]> GetPlaylistTrackIds(string id, CancellationToken cancellationToken = default)
        {
            
            try
            {
                using (_requestHelper.CreateClient(false))
                {
                    var requestUrl = Extensions.CombineUrls(_url, $"music/playlist/{id}");
                    var req = _requestHelper.CreateRequest(requestUrl, HTTPMethods.Get, true, false);
                    var res = await req.GetHTTPResponseAsync(cancellationToken);
                    var playlistsResponse = _serializer.DeserializeJson<PlaylistResponse>(res.DataAsText);
                    return playlistsResponse.playlist.tracks.Select(x => x.trackId).ToArray();
                }
            }
            catch (Exception)
            {
                return Array.Empty<string>();
            }
        }

        private async Task<UnityWebRequest> GetAsyncInternal(UnityWebRequest request, CancellationToken token)
        {
            var asyncOperation = request.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Delay(25, token);
                token.ThrowIfCancellationRequested();
            }
            
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }

            return request;
        }

        private ExternalTrackInfo[] GetSearchResult(TrackSearchResponse response)
        {
            return response.searchResults?.searchResult?.Select(x => MapTrack(x.track)).ToArray();
        }

        private async Task<UrlResult> GetUrl(string baseUrl, HTTPMethods method,
            Dictionary<string, string> parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryParameters = parameters ?? new Dictionary<string, string>();

                var body = new
                {
                    BaseUrl = baseUrl,
                    HttpMethod = method.ToString().ToLower(),
                    QueryParameters = queryParameters
                };

                var requestUrl = Extensions.CombineUrls(_url, "MusicProvider/SignUrl");
                var req = _requestHelper.CreateRequest(requestUrl, HTTPMethods.Post, true, false);
                var json = _serializer.SerializeToJson(body);
                req.AddJsonContent(json);

                var resp = await req.GetHTTPResponseAsync(cancellationToken);

                if (!resp.IsSuccess)
                {
                    return UrlResult.Error(resp.Message);
                }

                var url = _serializer.DeserializeJson<UrlResult>(resp.DataAsText);
                return url;
            }
            catch (OperationCanceledException)
            {
                return UrlResult.Canceled();
            }
        }

        private async Task<ExternalTrackInfo[]> GetPlaylistTracks(string[] trackIds, CancellationToken token)
        {
            var result = await GetTracks(trackIds, token);
            return result.Models;
        }

        private PlaylistInfo MapPlaylist(Playlist playlist, ExternalTrackInfo[] tracks)
        {
            return new PlaylistInfo
            {
                Title = playlist.name,
                Tracks = tracks
            };
        }

        private async Task<Result<PlaylistInfo>> GetPlaylistInfo(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestUrl = Extensions.CombineUrls(_url, $"music/playlist/{id}");
                var req = _requestHelper.CreateRequest(requestUrl, HTTPMethods.Get, true, false);
                var res = await req.GetHTTPResponseAsync(cancellationToken);

                if (!res.IsSuccess)
                {
                    return Result<PlaylistInfo>.Error(res.DataAsText);
                }
                
                var trackIds = await GetPlaylistTrackIds(id, cancellationToken);
                var tracks = await GetPlaylistTracks(trackIds, cancellationToken);
                
                var playlistResponse = _serializer.DeserializeJson<PlaylistResponse>(res.DataAsText);
                var playListInfo = MapPlaylist(playlistResponse.playlist, tracks);
                
                return Result<PlaylistInfo>.Success(playListInfo);
            }
            catch (OperationCanceledException)
            {
                return Result<PlaylistInfo>.Cancelled();
            }
        }

        private async Task<ArrayResult<string>> GetExternalPlaylistIds(string targetId, int takePrevious, int takeNext, CancellationToken cancellationToken = default)
        {
            try
            {
                var body = new
                {
                    Target = targetId,
                    TakePrevious = takePrevious,
                    TakeNext = takeNext
                };

                var requestUrl = Extensions.CombineUrls(_url, "external-playlists");
                var req = _requestHelper.CreateRequest(requestUrl, HTTPMethods.Post, true, false);
                var json = _serializer.SerializeToJson(body);
                req.AddJsonContent(json);
                
                var resp = await req.GetHTTPResponseAsync(cancellationToken);

                if (!resp.IsSuccess)
                {
                    return ArrayResult<string>.Error(resp.Message);
                }

                var externalPlaylistInfos = _serializer.DeserializeJson<ExternalPlaylistInfo[]>(resp.DataAsText);
                var ids = externalPlaylistInfos.OrderBy(x=>x.SortOrder).Select(x => x.ExternalPlaylistId).ToArray();
                return ArrayResult<string>.Success(ids);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<string>.Cancelled();
            }
        }
        
        private AuthenticationHeaderValue CreateAuthorizationHeader(string headerParameter)
        {
            return new AuthenticationHeaderValue("Bearer", Uri.EscapeDataString(headerParameter));
        }

        private int DurationToMilliseconds(int value)
        {
            return (int)Mathf.Clamp(value * 1000, 0, PREVIEW_CLIP_DURATION);
        }

        private ExternalTrackInfo MapTrack(Track track)
        {
            return new ExternalTrackInfo
            {
                Id = track.id,
                Duration = DurationToMilliseconds(track.duration),
                Title = track.title,
                ArtistName = track.artist.name,
                ThumbnailUrl = track.release.image,
                ExplicitContent = track.explicitContent,
            };
        }
    }
}
