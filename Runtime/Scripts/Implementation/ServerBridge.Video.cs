using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Results;
using Bridge.Services.UserProfile.PhoneLookup;
using Bridge.VideoServer;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<VideoUploadResult> UploadLevelVideoAsync(DeployLevelVideoReq req)
        {
            return _videoServer.UploadLevelVideoAsync(req);
        }

        public Task<VideoUploadResult> UploadNonLevelVideoAsync(DeployNonLevelVideoReq req)
        {
            return _videoServer.UploadNonLevelVideoAsync(req);
        }
        
        public Task<VideoUploadResult> UploadNonLevelAiVideoAsync(DeployNonLevelVideoReq req)
        {
            return _videoServer.UploadNonLevelAiVideoAsync(req);
        }
        
        public Task<Result> GenerateLipSync(long? videoId, LipSyncRequest lipSyncRequest)
        {
            return _videoServer.GenerateLipSync(videoId, lipSyncRequest);
        }

        public Task<VideoIdResult> GetNewestFeedVideoIdAsync(CancellationToken cancellationToken)
        {
            return _videoServer.GetNewestFeedVideoId(cancellationToken);
        }

        public Task<SingleEntityResult<Video>> GetVideoAsync(long videoId, CancellationToken cancellationToken)
        {
            return _videoServer.GetVideoAsync(videoId, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetVideoToRateListAsync(long levelId, CancellationToken token = default)
        {
            return _videoServer.GetVideoToRateListAsync(levelId, token);
        }

        public Task<EntitiesResult<Video>> GetMyVideoListAsync(long? targetVideo, int takeNextCount,
            int takePreviousCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetMyVideoListAsync(targetVideo, takeNextCount, takePreviousCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetFeedVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetFeedVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetTrendingVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetTrendingVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetFeaturedVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetFeaturedVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetMyFollowingVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetMyFollowingVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetMyFriendsVideoListAsync(string videoKey, int takeNextCount, CancellationToken cancellationToken = default)
        {
            return _videoServer.GetMyFriendsVideoListAsync(videoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetForYouFeedVideoListAsync(string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken = default)
        {
            return _videoServer.GetForYouFeedVideoListAsync(targetVideoKey, takeNextCount, cancellationToken);
        }
        
        public Task<EntitiesResult<Video>> GetForYouFeedMLVideoListAsync(string targetVideoKey, int takeNextCount,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken = default)
        {
            return _videoServer.GetForYouFeedMLVideoListAsync(targetVideoKey, takeNextCount, headers, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetTaskVideoListAsync(long taskId, string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken = default)
        {
            return _videoServer.GetTaskVideoListAsync(taskId, targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetUserTasksVideoListAsync(long userGroupId, string targetVideoKey, int takeNextCount,
            int takePreviousCount, CancellationToken cancellationToken = default)
        {
            return _videoServer.GetUserTasksVideoListAsync(userGroupId, targetVideoKey, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetRemixVideoListAsync(long videoId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetRemixVideoListAsync(videoId, targetVideo, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetUserTaggedVideoListAsync(long userGroup, long? targetVideo,
            int takeNextCount, int takePreviousCount = 0,
            CancellationToken cancellationToken = default)
        {
            return _videoServer.GetUserTaggedVideoListAsync(userGroup, targetVideo, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetPublicVideoForAccount(long groupId, long? targetVideo,
            int takeNextCount, int takePreviousCount, CancellationToken cancellationToken)
        {
            return _videoServer.GetPublicVideoForAccount(groupId, targetVideo, takeNextCount, takePreviousCount,
                cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetHashtagVideoListAsync(long hashtagId, string targetVideoKey, int takeNextCount,
            CancellationToken cancellationToken = default)
        {
            return _videoServer.GetVideosForHashtag(hashtagId, targetVideoKey, takeNextCount, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetVideoListBySoundAsync(SoundType soundType, long soundId, string targetVideoKey, int takeNext,
            CancellationToken cancellationToken = default)
        {
            return _videoServer.GetVideoListBySoundAsync(soundType, soundId, targetVideoKey, takeNext, cancellationToken);
        }

        public Task<EntitiesResult<TemplateChallenge>> GetTrendingTemplateChallenges(int take, int skip, int videoPerTemplate, CancellationToken cancellationToken)
        {
            return _videoServer.GetTrendingTemplateChallenges(take, skip, videoPerTemplate, cancellationToken);
        }

        public Task<EntitiesResult<Video>> GetVideoForTemplate(long templateId, string videoKey, int takeNext, CancellationToken cancellationToken)
        {
            return _videoServer.GetVideoForTemplate(templateId, videoKey, takeNext, cancellationToken);
        }

        public Task<VideoUrlResult> GetSharingUlr(long videoId, CancellationToken token)
        {
            return _videoServer.GetSharingUlr(videoId, token);
        }

        public Task<VideoUrlResult> GetVideoFileUlr(long videoId, CancellationToken token)
        {
            return _videoServer.GetVideoFileUlr(videoId, token);
        }

        public Task<Result<VideoShareInfo>> GetVideoShareInfo(string videoGuid, CancellationToken token = default)
        {
            return _videoServer.GetVideoShareInfo(videoGuid, token);
        }

        public Task<Result> LikeVideoAsync(long videoId)
        {
            return _videoServer.LikeVideoAsync(videoId);
        }

        public Task<Result> UnlikeVideoAsync(long videoId)
        {
            return _videoServer.UnlikeVideoAsync(videoId);
        }

        public Task<Result> DeleteVideo(long videoId)
        {
            return _videoServer.DeleteVideo(videoId);
        }

        public Task<Result> DeleteVideoByLevelId(long levelId)
        {
            return _videoServer.DeleteVideoByLevelId(levelId);
        }
        
        public Task<Result> ChangePrivacyAsync(long videoId, UpdateVideoAccessRequest model)
        {
            return _videoServer.ChangePrivacyAsync(videoId, model);
        }

        public Task<VideoReportResult> Report(long videoId, ReportData reportData)
        {
            return _videoReportService.Report(videoId, reportData);
        }

        public Task<ArrayResult<VideoReportReason>> GetReportReasons()
        {
            return _videoReportService.GetReportReasons();
        }

        public Task<Result> RateVideoList(RateVideoRequest requestModel)
        {
            return _videoServer.RateVideoList(requestModel);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoComments(long videoId, int take, int skip, CancellationToken token = default)
        {
            return _videoServer.GetVideoComments(videoId, skip, take, token);
        }

        public Task<SingleEntityResult<CommentInfo>> GetVideoComment(long videoId, long commentId, CancellationToken token = default)
        {
            return _videoServer.GetVideoComment(videoId, commentId, token);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoRootComments(long videoId, string key, int takeOlder, int takeNewer, CancellationToken token = default)
        {
            return _videoServer.GetVideoRootComments(videoId, key, takeOlder, takeNewer, token);
        }
        
        public Task<ArrayResult<CommentInfo>> GetVideoThreadComments(long videoId, string rootCommentKey, string replyCommentKey, int takeOlder, int takeNewer,
                                                                               CancellationToken token = default)
        {
            return _videoServer.GetVideoThreadComments(videoId, rootCommentKey, replyCommentKey, takeOlder, takeNewer, token);
        }

        public Task<SingleObjectResult<VideoCommentAuthorsInfo>> GetWhoCommentedTheVideo(long videoId, CancellationToken token = default)
        {
            return _videoServer.GetWhoCommentedTheVideo(videoId, token);
        }

        public Task<SingleEntityResult<CommentInfo>> AddComment(long videoId, AddCommentRequest request)
        {
            return _videoServer.AddComment(videoId, request);
        }
        
        public Task<Result> LikeCommentAsync(long videoId, long commentId)
        {
            return _videoServer.LikeCommentAsync(videoId, commentId);
        }

        public Task<Result> UnlikeCommentAsync(long videoId, long commentId)
        {
            return _videoServer.UnlikeCommentAsync(videoId, commentId);
        }

        public Task<Result> SendViewsData(ICollection<VideoView> views)
        {
            return _videoServer.SendViewsData(views);
        }

        public Task<ArrayResult<HashtagInfo>> GetHashtags(string filter, int skip, int take, CancellationToken cancellationToken = default)
        {
            return _videoServer.GetHashtags(filter, skip, take, cancellationToken);
        }
        
        public Task<Result<TemplateInfo>> GenerateTemplate(long videoId, string templateName)
        {
            return _videoServer.GenerateTemplate(videoId, templateName);
        }
        
        public Task<Result<long?>> CheckTemplateName(string templateName)
        {
            return _videoServer.CheckTemplateName(templateName);
        }

        public Task<FriendsByPhoneLookupResult> LookupForFriends(string[] phoneNumbers)
        {
            return _socialService.LookupForFriends(phoneNumbers);
        }
        
        public Task<Result<VideoSharingInfo>> GetVideoSharingInfo(long videoId, CancellationToken token = default)
        {
            return _videoServer.GetVideoSharingInfo(videoId, token);
        }

        public Task<Result> ClaimVideoShareReward(string videoGuid)
        {
            return _videoServer.ClaimVideoShareReward(videoGuid);
        }

        public Task<EntitiesResult<TransformationStyle>> GetVideoAiTransformationStyles()
        {
            return _videoServer.GetVideoAiTransformationStyles();
        }
    }
}