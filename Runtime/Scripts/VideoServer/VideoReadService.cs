using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.Common;
using Bridge.Models.VideoServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.VideoServer
{
    internal sealed partial class VideoReadService : VideoService
    {
        public VideoReadService(string host, IRequestHelper requestHelper, ISerializer serializer)
            : base(host, requestHelper, serializer)
        {
        }

        public Task<VideoUrlResult> GetSharingUlr(long videoId, CancellationToken token)
        {
            return GetVideoUrlInternal(videoId, "player-url",token);
        }
        
        public Task<VideoUrlResult> GetVideoFileUlr(long videoId, CancellationToken token)
        {
            return GetVideoUrlInternal(videoId, "file-url", token);
        }

        public async Task<EntitiesResult<Video>> GetVideoToRateListAsync(long levelId, CancellationToken token)
        {
            try
            {
                var url = Extensions.CombineUrls(Host, $"rating/{levelId}");
                return await GetVideoListAsync(url, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetUserTaggedVideoListAsync(long userGroupId, long? targetVideo,
            int takeNextCount, int takePreviousCount,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetUserTaggedVideoListInternal(userGroupId, targetVideo, takeNextCount, takePreviousCount,
                        cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }
        
        public async Task<EntitiesResult<Video>> GetVideosForHashtag(long hashtagId, string targetVideoKey, int takeNextCount,
                                                                             CancellationToken cancellationToken = default)
        {
            try
            {
                return await Task.Run(() => GetVideosForHashtagInternal(hashtagId, targetVideoKey, takeNextCount, cancellationToken),
                                      cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<SingleEntityResult<Video>> GetVideoAsync(long videoId, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetVideoAsyncInternal(videoId, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledSingleEntityResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetPublicVideoForAccount(long groupId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(
                    () => GetPublicVideoForAccountInternal(groupId, targetVideo, takeNextCount, takePreviousCount,
                        cancellationToken),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetVideoListBySoundAsync(SoundType soundType, long soundId,
            string targetVideo, int takeNext, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetVideoListBySoundAsyncInternal(soundType, soundId, targetVideo, takeNext,
                    cancellationToken);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return new CanceledEntitiesesResult<Video>();
                return new EntitiesResult<Video>(e.Message);
            }
        }
        
        public async Task<EntitiesResult<Video>> GetRemixVideoListAsync(long videoId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(
                    () => GetRemixVideoListAsyncInternal(videoId, targetVideo, takeNextCount, takePreviousCount,
                        cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetMyVideoListAsync(long? targetVideo, int takeNextCount,
            int takePreviousCount, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(
                    () => GetMyVideoListAsyncInternal(targetVideo, takeNextCount, takePreviousCount, cancellationToken),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetForYouFeedVideoListAsync(string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetForYouVideoListAsyncInternal(targetVideoKey, takeNextCount, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }
        
        public async Task<EntitiesResult<Video>> GetForYouFeedMLVideoListAsync(string targetVideoKey, int takeNextCount,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetForYouMLVideoListAsyncInternal(targetVideoKey, takeNextCount, headers, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetTaskVideoListAsync(long taskId, string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken)
        {
            try
            {
                return await GetTaskVideoListAsyncInternal(taskId, targetVideoKey, takeNextCount, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetUserTasksVideoListAsync(long userGroupId, string targetVideoKey,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetUserTasksVideoListAsyncInternal(userGroupId, targetVideoKey, takeNextCount, takePreviousCount, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetFeedVideoListAsync(string targetVideo, int takeNextCount, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(
                    () => GetFeedVideoListAsyncInternal(targetVideo, takeNextCount, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }
        
        public async Task<EntitiesResult<Video>> GetMyFriendsVideoListAsync(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(
                    () => GetVideoListAsync("video/my-friends-videos", targetVideo, takeNextCount, token: cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetTrendingVideoListAsync(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetVideoListAsync("video/trending",targetVideo, takeNextCount, token: cancellationToken),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetFeaturedVideoListAsync(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetVideoListAsync("video/featured", targetVideo, takeNextCount, token: cancellationToken),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<EntitiesResult<Video>> GetMyFollowingVideoListAsync(string targetVideoKey, int takeNext, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetVideoListAsync("video/my-following", targetVideoKey, takeNext, token: cancellationToken),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<VideoIdResult> GetNewestFeedVideoId(CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetNewestFeedVideoIdInternal(cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledVideoIdResult();
            }
        }

        public async Task<EntitiesResult<TemplateChallenge>> GetTrendingTemplateChallenges(int take, int skip, int videoPerTemplate, CancellationToken cancellationToken)
        {
            try
            {
                return await GetTrendingTemplateChallengesInternal(take, skip, videoPerTemplate, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<TemplateChallenge>();
            }
        }

        public async Task<EntitiesResult<Video>> GetVideoForTemplate(long templateId, string videoKey, int takeNext, CancellationToken cancellationToken)
        {
            try
            {          
                return await GetVideoForTemplateInternal(templateId, videoKey, takeNext, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<Video>();
            }
        }

        public async Task<Result<VideoShareInfo>> GetVideoShareInfo(string videoGuid, CancellationToken token)
        {
            try
            {          
                var url = Path.Combine(Host, $"video/watch/{videoGuid}");
                var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
                var resp = await req.GetHTTPResponseAsync(token: token);
                if (!resp.IsSuccess)
                {
                    return Result<VideoShareInfo>.Error(resp.DataAsText);
                }

                var model = Serializer.DeserializeJson<VideoShareInfo>(resp.DataAsText);
                return Result<VideoShareInfo>.Success(model);
            }
            catch (OperationCanceledException)
            {
                return Result<VideoShareInfo>.Cancelled();
            }
        }

        public async Task<Result<VideoSharingInfo>> GetVideoSharingInfo(long videoId, CancellationToken token)
        {
            try
            {          
                var url = Path.Combine(Host, $"video/{videoId}/sharing-info");
                var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
                var resp = await req.GetHTTPResponseAsync(token: token);
                if (!resp.IsSuccess)
                {
                    return Result<VideoSharingInfo>.Error(resp.DataAsText);
                }

                var model = Serializer.DeserializeJson<VideoSharingInfo>(resp.DataAsText);
                return Result<VideoSharingInfo>.Success(model);
            }
            catch (OperationCanceledException)
            {
                return Result<VideoSharingInfo>.Cancelled();
            }
        }

        public async Task<EntitiesResult<TransformationStyle>> GetVideoAiTransformationStyles()
        {
            var url = Extensions.CombineUrls(Host, "style-transformation/style");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new EntitiesResult<TransformationStyle>(resp.DataAsText);
            }

            var styles = Serializer.DeserializeProtobuf<TransformationStyle[]>(resp.Data);
            return new EntitiesResult<TransformationStyle>(styles);
        }

        private Task<EntitiesResult<Video>> GetVideoForTemplateInternal(long templateId, string videoKey, int takeNext, CancellationToken cancellationToken)
        {
            var url = Path.Combine(Host, $"video/template/{templateId}?$targetVideo={videoKey}&$takeNext={takeNext}");
            return GetVideoListAsync(url, cancellationToken);
        }

        private string GetUrlForVideoQuery(Query<Video> query, string apiUrl)
        {
            var url = Path.Combine(Host, apiUrl);

            if (query != null)
            {
                var queryString = query.BuildQuery();
                url += queryString;
            }

            return url.FixUrlSlashes();
        }

        private Task<EntitiesResult<Video>> GetVideoListAsync(string apiUrl, string videoKey, int takeNext, int? takePrevious = null,
            CancellationToken token = default)
        {
            var query = CreateVideoPaginationQuery(videoKey, takeNext, takePrevious);
            var url = GetUrlForVideoQuery(query, apiUrl);
            return GetVideoListAsync(url, token);
        }
        
        private Task<EntitiesResult<Video>> GetVideoListAsync(Query<Video> query, string apiUrl,
            CancellationToken cancellationToken,
            IDictionary<string, string> headers = null)
        {
            var url = GetUrlForVideoQuery(query, apiUrl);
            return GetVideoListAsync(url, cancellationToken, headers);
        }

        private async Task<EntitiesResult<Video>> GetVideoListAsync(string url, CancellationToken cancellationToken,
            IDictionary<string, string> headers = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            req.AddHeaders(headers);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);

            if (!resp.IsSuccess)
            {
                var error = resp.DataAsText;
                return new EntitiesResult<Video>(error);
            }

            var videoList = Serializer.DeserializeProtobuf<Video[]>(resp.Data);
            return new EntitiesResult<Video>(videoList);
        }
        
        private async Task<VideoIdResult> GetNewestFeedVideoIdInternal(CancellationToken cancellationToken)
        {
            var url = GetUrlForVideoQuery(null, "video/feed/newestId");

            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);

            if (!resp.IsSuccess)
            {
                var error = resp.DataAsText;
                return new VideoIdResult(error);
            }

            var stringId = resp.DataAsText;
            var id = long.Parse(stringId);
            return new VideoIdResult(id);
        }

        private Task<EntitiesResult<Video>> GetMyVideoListAsyncInternal(long? targetVideo, int takeNextCount,
            int takePreviousCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideo, takeNextCount, takePreviousCount);
            return GetVideoListAsync(query, "video/my-videos", cancellationToken);
        }
        
        private Task<EntitiesResult<Video>> GetForYouVideoListAsyncInternal(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideo, takeNextCount);
            return GetVideoListAsync(query, "video/fyp", cancellationToken);
        }
        
        private Task<EntitiesResult<Video>> GetForYouMLVideoListAsyncInternal(string targetVideo, int takeNextCount, IDictionary<string, string> headers, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideo, takeNextCount);
            return GetVideoListAsync(query, "video/fyp-v2", cancellationToken, headers);
        }

        private Task<EntitiesResult<Video>> GetFeedVideoListAsyncInternal(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideoKey, takeNextCount);
            return GetVideoListAsync(query, "video/new", cancellationToken);
        }
        
        private VideoQuery CreateVideoPaginationQuery(string targetVideo, int takeNextCount)
        {
            var query = new VideoQuery();
            query.SetStartFromVideo(targetVideo);
            query.SetTakeNext(takeNextCount);
            return query;
        }
        
        private VideoQuery CreateVideoPaginationQuery(long? targetVideo, int takeNextCount, int takePreviousCount)
        {
            var query = new VideoQuery();
            query.SetStartFromVideo(targetVideo);
            query.SetTakeNext(takeNextCount);
            query.SetTakePrevious(takePreviousCount);
            return query;
        }
        
        private VideoQuery CreateVideoPaginationQuery(string targetVideoKey, int takeNextCount, int? takePreviousCount)
        {
            var query = new VideoQuery();
            query.SetStartFromVideo(targetVideoKey);
            query.SetTakeNext(takeNextCount);
            if (takePreviousCount.HasValue)
            {
                query.SetTakePrevious(takePreviousCount.Value);
            }
            return query;
        }

        private Query<T> CreateTakeSkipQuery<T>(int take, int skip) where T : IEntity
        {
            var q = new Query<T>();
            q.SetSkip(skip);
            q.SetMaxTop(take);
            return q;
        }
        
        private async Task<SingleEntityResult<Video>> GetVideoAsyncInternal(long videoId,
            CancellationToken cancellationToken)
        {
            var url = GetUrlForVideoQuery(null, $"video/{videoId}");

            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);

            if (!resp.IsSuccess)
            {
                var error = resp.DataAsText;
                return new SingleEntityResult<Video>(error);
            }

            var model = Serializer.DeserializeProtobuf<Video>(resp.Data);
            return new SingleEntityResult<Video>(model);
        }
        
        private Task<EntitiesResult<Video>> GetUserTaggedVideoListInternal(long groupId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideo, takeNextCount, takePreviousCount);
            return GetVideoListAsync(query, $"video/tagged/{groupId}", cancellationToken);
        }

        private Task<EntitiesResult<Video>> GetPublicVideoForAccountInternal(long groupId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideo, takeNextCount, takePreviousCount);
            return GetVideoListAsync(query, $"video/by-group/{groupId}", cancellationToken);
        }
        
        private Task<EntitiesResult<Video>> GetRemixVideoListAsyncInternal(long videoId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideo, takeNextCount, takePreviousCount);
            return GetVideoListAsync(query, $"video/{videoId}/remixes", cancellationToken);
        }
        
        private async Task<EntitiesResult<TemplateChallenge>> GetTrendingTemplateChallengesInternal(int take, int skip, int videoPerTemplate, CancellationToken cancellationToken)
        {
            var url = Path.Combine(Host,
                $"template/trending-challenges?top={take}&skip={skip}&videoCount={videoPerTemplate}");
            
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var response = await req.GetHTTPResponseAsync(cancellationToken);
            if (!response.IsSuccess)
            {
                var error = response.DataAsText;
                return new EntitiesResult<TemplateChallenge>(error);
            }

            var templates = Serializer.DeserializeProtobuf<TemplateChallenge[]>(response.Data);
            return new EntitiesResult<TemplateChallenge>(templates);
        }

        private async Task<VideoUrlResult> GetVideoUrlInternal(long videoId, string endPoint, CancellationToken  token)
        {
            try
            {
                var url = Path.Combine(Host, $"video/{videoId}/{endPoint}");
                var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
                var resp = await request.GetHTTPResponseAsync(token);
                if (!resp.IsSuccess) return VideoUrlResult.Error(resp.DataAsText);
                var sharingUrl = resp.DataAsText;
                return VideoUrlResult.Success(sharingUrl);
            }
            catch (OperationCanceledException)
            {
                return VideoUrlResult.Canceled();
            }
        }
        
        private Task<EntitiesResult<Video>> GetVideosForHashtagInternal(long hashtagId, string targetVideoKey,
                                                                           int takeNextCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideoKey, takeNextCount);
            return GetVideoListAsync(query, $"video/hashtag/{hashtagId}", cancellationToken);
        }

        private Task<EntitiesResult<Video>> GetTaskVideoListAsyncInternal(long taskId, string targetVideoKey,
            int takeNextCount, CancellationToken cancellationToken)
        {
            var query = CreateVideoPaginationQuery(targetVideoKey, takeNextCount);
            return GetVideoListAsync(query, $"video/by-task/{taskId}", cancellationToken);
        }

        private Task<EntitiesResult<Video>> GetUserTasksVideoListAsyncInternal(long userGroupId, string targetVideoKey,
            int takeNextCount, int takePreviousCount,
            CancellationToken cancellationToken = default)
        {
            var query = CreateVideoPaginationQuery(targetVideoKey, takeNextCount, takePreviousCount);
            return GetVideoListAsync(query, $"video/by-group/{userGroupId}/tasks", cancellationToken);
        }

        private Task<EntitiesResult<Video>> GetVideoListBySoundAsyncInternal(SoundType soundType, long soundId,
            string targetVideoKey, int takeNext, CancellationToken cancellationToken = default)
        {
            var query = CreateVideoPaginationQuery(targetVideoKey, takeNext);
            return GetVideoListAsync(query, $"video/sound/{soundId}/{(int)soundType}?targetVideo={targetVideoKey}&takeNext={takeNext}", cancellationToken);
        }
    }
}