using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Gamification;
using Bridge.Models.ClientServer.UserActivity;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Rewards
{
    internal interface IRewardClaimingService
    {
        Task<ClaimRewardResult> ClaimDailyQuestReward(long dailyQuestId);
        Task<ClaimRewardResult> ClaimLevelReward(long levelRewardId);
        Task<ClaimRewardResult> ClaimSeasonQuestReward(long seasonQuestId);
        Task<ClaimRewardResult> ClaimCreatorScoreReward(long rewardId);
        Task<ClaimRewardResult> ClaimVotingBattleReward(long taskId);
        Task<ClaimRewardResult> ClaimOnboardingReward(long onboardingRewardId);
        Task<ClaimRewardResult> ClaimTrophyHuntReward(long crewRewardId);
        Task<ClaimRewardResult> ClaimVideoRaterReward(long videoId);
        Task<ClaimRewardResult> ClaimRatedVideoReward(long videoId);
        Task<Result<ClaimPastRewardsResult>> ClaimPastRewards();
        Task<Result> ClaimRewardForInvitedUser();
        Task<Result> ClaimRewardFromInvitedUser(long invitedUserGroupId);
    }

    internal interface ISeasonService
    {
        Task<Result<CurrentSeason>> GetCurrentSeason(CancellationToken token);
    }
    
    internal sealed class SeasonService: ServiceBase, IRewardClaimingService, ISeasonService
    {
        private const string END_POINT_BASE = "gamification";
        
        public SeasonService(string host, IRequestHelper requestHelper, ISerializer serializer) 
            : base(host, requestHelper, serializer)
        {
        }

        public Task<ClaimRewardResult> ClaimDailyQuestReward(long dailyQuestId)
        {
            var endPoint = $"daily-quest/{dailyQuestId}";
            return ClaimReward(endPoint);
        }

        public Task<ClaimRewardResult> ClaimLevelReward(long levelRewardId)
        {
            var endPoint = $"level/{levelRewardId}";
            return ClaimReward(endPoint);
        }

        public Task<ClaimRewardResult> ClaimSeasonQuestReward(long seasonQuestId)
        {
            var endPoint = $"season-quest/{seasonQuestId}";
            return ClaimReward(endPoint);
        }

        public Task<ClaimRewardResult> ClaimVotingBattleReward(long taskId)
        {
            var endPoint = $"battle/{taskId}";
            return ClaimReward(endPoint);
        }

        public Task<ClaimRewardResult> ClaimOnboardingReward(long onboardingRewardId)
        {
            var endPoint = $"onboarding/{onboardingRewardId}";
            return ClaimReward(endPoint);
        }

		public Task<ClaimRewardResult> ClaimTrophyHuntReward(long crewRewardId)
        {
            var endPoint = $"crew/{crewRewardId}";
            return ClaimReward(endPoint);
        }

        public Task<ClaimRewardResult> ClaimVideoRaterReward(long videoId)
        {
            var endPoint = $"video-rater/{videoId}";
            return ClaimReward(endPoint);
        }
        
        public Task<ClaimRewardResult> ClaimRatedVideoReward(long videoId)
        {
            var endPoint = $"rated-video/{videoId}";
            return ClaimReward(endPoint);
        }

        public async Task<Result<ClaimPastRewardsResult>> ClaimPastRewards()
        {
            var url = ConcatUrl(Host, $"{END_POINT_BASE}/reward/past");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<ClaimPastRewardsResult>.Error(resp.DataAsText);
            }

            var model = Serializer.DeserializeJson<ClaimPastRewardsResult>(resp.DataAsText);
            return Result<ClaimPastRewardsResult>.Success(model);
        }

        public Task<Result> ClaimRewardForInvitedUser()
        {
            var url = ConcatUrl(Host, $"{END_POINT_BASE}/reward/invitee");
            return SendPostRequest(url);
        }

        public Task<Result> ClaimRewardFromInvitedUser(long invitedUserGroupId)
        {
            var url = ConcatUrl(Host, $"{END_POINT_BASE}/reward/inviter/{invitedUserGroupId}");
            return SendPostRequest(url);
        }

        private async Task<ClaimRewardResult> ClaimReward(string endPoint)
        {
            var url = ConcatUrl(Host, $"{END_POINT_BASE}/reward/{endPoint}");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return ClaimRewardResult.Error(resp.DataAsText);
            }

            var model = Serializer.DeserializeJson<ClaimRewardResultDto>(resp.DataAsText);
            return !model.Ok ? ClaimRewardResult.Error(model.ErrorCode) : ClaimRewardResult.Success(model);
        }
        
        public Task<ClaimRewardResult> ClaimCreatorScoreReward(long rewardId)
        {
            var endPoint = $"creator-level/{rewardId}";
            return ClaimReward(endPoint);
        }

        public Task<Result<CurrentSeason>> GetCurrentSeason(CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{END_POINT_BASE}/current-season");
            return SendRequestForSingleModel<CurrentSeason>(url, token, true);
        }
    }
}
