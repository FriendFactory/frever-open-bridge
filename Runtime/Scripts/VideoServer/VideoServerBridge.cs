using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.VideoServer.HashTags;

namespace Bridge.VideoServer
{
    internal sealed class VideoServerBridge : IVideoServer
    {
        private readonly VideoWriteService _writeService;
        private readonly VideoReadService _readService;
        private readonly HashTagsReadService _hashTagsReadService; 

        public VideoServerBridge(string hostUrl, IRequestHelper requestHelper, ISerializer serializer)
        {
            _writeService = new VideoWriteService(hostUrl, requestHelper, serializer);
            _readService = new VideoReadService(hostUrl, requestHelper, serializer);
            _hashTagsReadService = new HashTagsReadService(hostUrl, requestHelper, serializer);
        }

        public Task<VideoUploadResult> UploadLevelVideoAsync(DeployLevelVideoReq req)
        {
            return _writeService.UploadLevelVideoAsync(req);
        }

        public Task<VideoUploadResult> UploadNonLevelVideoAsync(DeployNonLevelVideoReq req)
        {
            return _writeService.UploadNonLevelVideoAsync(req);
        }

        public Task<VideoUploadResult> UploadNonLevelAiVideoAsync(DeployNonLevelVideoReq req)
        {
            return _writeService.UploadNonLevelAiVideoAsync(req);
        }

        public Task<Result> GenerateLipSync(long? videoId, LipSyncRequest lipSyncRequest)
        {
            return _writeService.GenerateLipSync(videoId, lipSyncRequest);
        }

        public Task<VideoUrlResult> GetSharingUlr(long videoId, CancellationToken token)
        {
            return _readService.GetSharingUlr(videoId, token);
        }

        public Task<VideoUrlResult> GetVideoFileUlr(long videoId, CancellationToken token)
        {
            return _readService.GetVideoFileUlr(videoId, token);
        }

        public Task<EntitiesResult<Video>> GetVideoToRateListAsync(long levelId, CancellationToken token = default)
        {
            return _readService.GetVideoToRateListAsync(levelId, token);
        }

        public Task<EntitiesResult<Video>> GetUserTaggedVideoListAsync(long userGroupId, long? targetVideo, int takeNextCount, int takePreviousCount,
            CancellationToken cancellationToken = default)
        {
            return _readService.GetUserTaggedVideoListAsync(userGroupId, targetVideo, takeNextCount, takePreviousCount,
                cancellationToken);
        }
        
        public Task<EntitiesResult<Video>> GetVideosForHashtag(long hashtagId, string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            return _readService.GetVideosForHashtag(hashtagId, targetVideo, takeNextCount, cancellationToken);
        }

        public Task<SingleEntityResult<Video>> GetVideoAsync(long videoId, CancellationToken cancellationToken)
        {
            return _readService.GetVideoAsync(videoId, cancellationToken);
        }

        public Task<EntitiesResult<TemplateChallenge>> GetTrendingTemplateChallenges(int take, int skip, int videoPerTemplate, CancellationToken cancellationToken)
        {
            return _readService.GetTrendingTemplateChallenges(take, skip, videoPerTemplate, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetVideoForTemplate(long templateId, string videoKey, int takeNext, CancellationToken cancellationToken)
        {
            return _readService.GetVideoForTemplate(templateId, videoKey, takeNext, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetMyFriendsVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default)
        {
            return _readService.GetMyFriendsVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetPublicVideoForAccount(long groupId, long? targetVideo, int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            return _readService.GetPublicVideoForAccount(groupId, targetVideo, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetVideoListBySoundAsync(SoundType soundType, long soundId, string targetVideoKey, int takeNext,
            CancellationToken cancellationToken = default)
        {
            return _readService.GetVideoListBySoundAsync(soundType, soundId, targetVideoKey, takeNext, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetRemixVideoListAsync(long videoId, long? targetVideo, int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            return _readService.GetRemixVideoListAsync(videoId, targetVideo, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetMyVideoListAsync(long? targetVideo, int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            return _readService.GetMyVideoListAsync(targetVideo, takeNextCount, takePreviousCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetFeedVideoListAsync(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            return _readService.GetFeedVideoListAsync(targetVideo, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetForYouFeedVideoListAsync(string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken = default)
        {
            return _readService.GetForYouFeedVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }
        
        public Task<EntitiesResult<Video>> GetForYouFeedMLVideoListAsync(string targetVideoKey, int takeNextCount,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken = default)
        {
            return _readService.GetForYouFeedMLVideoListAsync(targetVideoKey, takeNextCount, headers, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetTaskVideoListAsync(long taskId, string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken)
        {
            return _readService.GetTaskVideoListAsync(taskId, targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetUserTasksVideoListAsync(long userGroupId, string targetVideoKey, int takeNextCount,
            int takePreviousCount, CancellationToken cancellationToken)
        {
            return _readService.GetUserTasksVideoListAsync(userGroupId, targetVideoKey, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetTrendingVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken)
        {
            return _readService.GetTrendingVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetFeaturedVideoListAsync(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            return _readService.GetFeaturedVideoListAsync(targetVideo, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetMyFollowingVideoListAsync(string targetVideo, int takeNextCount, CancellationToken cancellationToken)
        {
            return _readService.GetMyFollowingVideoListAsync(targetVideo, takeNextCount, cancellationToken);
        }

        public Task<Result> LikeVideoAsync(long videoId)
        {
            return _writeService.LikeVideoAsync(videoId);
        }

        public Task<Result> UnlikeVideoAsync(long videoId)
        {
            return _writeService.UnlikeVideoAsync(videoId);
        }

        public Task<Result> ChangePrivacyAsync(long videoId, UpdateVideoAccessRequest model)
        {
            return _writeService.ChangePrivacyAsync(videoId, model);
        }

        public Task<Result> DeleteVideo(long videoId)
        {
            return _writeService.DeleteVideo(videoId);
        }

        public Task<Result> DeleteVideoByLevelId(long levelId)
        {
            return _writeService.DeleteVideoByLevelId(levelId);
        }

        public Task<Result> RateVideoList(RateVideoRequest requestModel)
        {
            return _writeService.RateVideoList(requestModel);
        }

        public Task<VideoIdResult> GetNewestFeedVideoId(CancellationToken cancellationToken)
        {
            return _readService.GetNewestFeedVideoId(cancellationToken);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoComments(
            long videoId,
            int skip,
            int take,
            CancellationToken token = default
        )
        {
            return _readService.GetVideoComments(videoId, skip, take, default);
        }

        public Task<SingleEntityResult<CommentInfo>> GetVideoComment(long videoId, long commentId, CancellationToken token = default)
        {
            return _readService.GetVideoComment(videoId, commentId, token);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoRootComments(long videoId, string key, int takeOlder, int takeNewer, CancellationToken token = default)
        {
            return _readService.GetVideoRootComments(videoId, key, takeOlder, takeNewer, token);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoThreadComments(long videoId, string rootCommentKey, string replyCommentKey, int takeOlder, int takeNewer,
            CancellationToken token = default)
        {
            return _readService.GetVideoThreadComments(videoId, rootCommentKey, replyCommentKey, takeOlder, takeNewer, token);
        }

        public Task<SingleObjectResult<VideoCommentAuthorsInfo>> GetWhoCommentedTheVideo(long videoId, CancellationToken token)
        {
            return _readService.GetWhoCommentedTheVideo(videoId, token);
        }

        public Task<SingleEntityResult<CommentInfo>> AddComment(long videoId, AddCommentRequest request)
        {
            return _writeService.AddComment(videoId, request);
        }
        
        public Task<Result> LikeCommentAsync(long videoId, long commentId)
        {
            return _writeService.LikeCommentAsync(videoId, commentId);
        }

        public Task<Result> UnlikeCommentAsync(long videoId, long commentId)
        {
            return _writeService.UnlikeCommentAsync(videoId, commentId);
        }

        public Task<Result> SendViewsData(ICollection<VideoView> views)
        {
            return _writeService.SendViewsData(views);
        }

        public Task<ArrayResult<HashtagInfo>> GetHashtags(string filter, int skip, int take, CancellationToken cancellationToken)
        {
            return _hashTagsReadService.GetHashtags(filter, skip, take, cancellationToken);
        }

        public Task<Result<TemplateInfo>> GenerateTemplate(long videoId, string templateName)
        {
            return _writeService.GenerateTemplate(videoId, templateName);
        }
        
        public Task<Result<long?>> CheckTemplateName(string templateName)
        {
            return _writeService.CheckTemplateName(templateName);
        }

        public Task<Result<VideoShareInfo>> GetVideoShareInfo(string videoGuid, CancellationToken token = default)
        {
            return _readService.GetVideoShareInfo(videoGuid, token);
        }
        
        public Task<Result<VideoSharingInfo>> GetVideoSharingInfo(long videoId, CancellationToken token = default)
        {
            return _readService.GetVideoSharingInfo(videoId, token);
        }

        public Task<Result> ClaimVideoShareReward(string videoGuid)
        {
            return _writeService.ClaimVideoShareReward(videoGuid);
        }

        public Task<EntitiesResult<TransformationStyle>> GetVideoAiTransformationStyles()
        {
            return _readService.GetVideoAiTransformationStyles();
        }
    }
}