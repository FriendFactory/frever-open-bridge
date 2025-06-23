using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Results;
using Bridge.VideoServer;

namespace Bridge
{
    public interface IVideoBridge
    {
        Task<VideoUploadResult> UploadLevelVideoAsync(DeployLevelVideoReq req);
        Task<VideoUploadResult> UploadNonLevelVideoAsync(DeployNonLevelVideoReq req);
        Task<VideoUploadResult> UploadNonLevelAiVideoAsync(DeployNonLevelVideoReq req);
        Task<Result> GenerateLipSync(long? videoId, LipSyncRequest lipSyncRequest);
        Task<EntitiesResult<Video>> GetMyVideoListAsync(long? targetVideo, int takeNextCount, int takePreviousCount = 0, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetFeedVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<VideoIdResult> GetNewestFeedVideoIdAsync(CancellationToken cancellationToken = default);
        Task<SingleEntityResult<Video>> GetVideoAsync(long videoId, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetVideoToRateListAsync(long levelId, CancellationToken token = default);
        Task<EntitiesResult<Video>> GetTrendingVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetFeaturedVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetMyFollowingVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetMyFriendsVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetForYouFeedVideoListAsync(string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetForYouFeedMLVideoListAsync(string targetVideoKey, int takeNextCount, IDictionary<string, string> headers, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetTaskVideoListAsync(long taskId, string targetVideoKey, int takeNextCount, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetUserTasksVideoListAsync(long userGroupId, string targetVideoKey, int takeNextCount, int takePreviousCount = 0, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetRemixVideoListAsync(long videoId, long? targetVideo, int takeNextCount, int takePreviousCount = 0, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetUserTaggedVideoListAsync(long userGroup, long? targetVideo, int takeNextCount, int takePreviousCount = 0, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetPublicVideoForAccount(long groupId, long? targetVideo, int takeNextCount, int takePreviousCount = 0, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetHashtagVideoListAsync(long hashtagId, string targetVideoKey, int takeNext, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetVideoListBySoundAsync(SoundType soundType, long soundId, string targetVideoKey, int takeNext, CancellationToken cancellationToken = default);
        Task<EntitiesResult<TemplateChallenge>> GetTrendingTemplateChallenges(int take, int skip, int videoPerTemplate, CancellationToken cancellationToken = default);
        Task<EntitiesResult<Video>> GetVideoForTemplate(long templateId, string videoKey, int takeNext, CancellationToken cancellationToken = default);
        Task<VideoUrlResult> GetSharingUlr(long videoId, CancellationToken token = default);
        Task<VideoUrlResult> GetVideoFileUlr(long videoId, CancellationToken token = default);
        Task<Result<VideoShareInfo>> GetVideoShareInfo(string videoGuid, CancellationToken token = default);
        
        Task<Result> LikeVideoAsync(long videoId);
        Task<Result> UnlikeVideoAsync(long videoId);
        Task<Result> DeleteVideo(long videoId);
        Task<Result> DeleteVideoByLevelId(long levelId);
        Task<Result> ChangePrivacyAsync(long videoId, UpdateVideoAccessRequest model);
        Task<VideoReportResult> Report(long videoId, ReportData reportData);
        Task<ArrayResult<VideoReportReason>> GetReportReasons();
        Task<Result> RateVideoList(RateVideoRequest requestModel);
        
        Task<ArrayResult<CommentInfo>> GetVideoComments(long videoId, int take, int skip, CancellationToken token = default);
        Task<SingleEntityResult<CommentInfo>> GetVideoComment(long videoId, long commentId, CancellationToken token = default);
        Task<ArrayResult<CommentInfo>> GetVideoRootComments(long videoId, string key, int takeOlder, int takeNewer, CancellationToken token = default);
        Task<ArrayResult<CommentInfo>> GetVideoThreadComments(long videoId, string rootCommentKey, string replyCommentKey, int takeOlder, int takeNewer, CancellationToken token = default);
        Task<SingleObjectResult<VideoCommentAuthorsInfo>> GetWhoCommentedTheVideo(long videoId, CancellationToken token = default);
        Task<Result> LikeCommentAsync(long videoId, long commentId);
        Task<Result> UnlikeCommentAsync(long videoId, long commentId);

        Task<SingleEntityResult<CommentInfo>> AddComment(long videoId, AddCommentRequest request);
        Task<Result> SendViewsData(ICollection<VideoView> views);

        Task<ArrayResult<HashtagInfo>> GetHashtags(string filter, int skip, int take,
            CancellationToken cancellationToken = default);
        Task<Result<TemplateInfo>> GenerateTemplate(long videoId, string templateName);
        Task<Result<long?>> CheckTemplateName(string templateName);
        Task<Result<VideoSharingInfo>> GetVideoSharingInfo(long videoId, CancellationToken token = default);
        Task<Result> ClaimVideoShareReward(string videoGuid);
        Task<EntitiesResult<TransformationStyle>> GetVideoAiTransformationStyles();
    }
}