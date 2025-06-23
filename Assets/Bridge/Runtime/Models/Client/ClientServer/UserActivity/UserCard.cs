using System;
using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer.UserActivity
{
    public class UserCard
    {
        public bool IsPremium { get; set; }

        public XpInfo Xp { get; set; }

        public int LikesReceivedToday { get; set; }

        public int LikesReceivedThisSeason { get; set; }

        public DailyQuestLikes[] DailyQuestLikes { get; set; }

        public long[] RewardClaimed { get; set; }

        public long[] CreatorLevelRewardClaimed { get; set; }

        public bool IsStarCreator { get; set; }

        public int TotalVideos { get; set; }

        public int TotalLikes { get; set; }

        public int TotalFollowers { get; set; }

        public int CreatorScoreBadge { get; set; }

        public long CreatorScore { get; set; }

        public long? CreatorRank { get; set; }

        public bool AllowCreateTemplateFromVideo { get; set; }

        public long[] OnboardingRewardClaimed { get; set; }

        public OnboardingQuestProgress[] OnboardingQuestCompletion { get; set; }
        
        [ProtoNewField(1)] public bool AllowVideoToFeed { get; set; }
        [ProtoNewField(2)] public bool AllowCrewCreation { get; set; }
    }

    public class DailyQuestLikes
    {
        public long DailyQuestId { get; set; }

        public int LikesToReceive { get; set; }

        public int Xp { get; set; }

        public int? SoftCurrencyPayout { get; set; }

        public bool IsClaimed { get; set; }
    }

    public class XpInfo
    {
        public UserLevelInfo PreviousLevel { get; set; }

        public UserLevelInfo CurrentLevel { get; set; }

        public UserLevelInfo NextLevel { get; set; }

        public int Xp { get; set; }
    }

    public class UserLevelInfo
    {
        public int Level { get; set; }

        public int Xp { get; set; }
    }
    
    public class OnboardingQuestProgress
    {
        public long OnboardingQuestId { get; set; }

        public bool IsCompleted { get; set; }

        public int CurrentProgress { get; set; }

        public int ToComplete { get; set; }

        public string QuestType { get; set; }
    }
}