namespace Bridge.Models.ClientServer.UserActivity
{
    public enum UserActionType
    {
        WatchVideo = 1,
        WatchVideoStreak = 10,
        CompleteTask = 2,
        OriginalVideoCreated = 3,
        TemplateVideoCreated = 4,
        LikeVideo = 5,
        LikeVideoStreak = 9,
        Login = 6,
        LoginStreak = 7,
        OriginalVideoCreationStreak = 8,
        LikeReceived = 11,
        LikeReceivedStreak = 12,
        DailyQuestRewardClaimed = 13,
        LevelUpRewardClaimed = 14,
        SeasonQuestRewardClaimed = 15,
        CreatorLevelRewardClaimed = 16,
        BattleRewardClaimed = 17,
        InvitationCodeRewardClaimed = 18,
        UpdateUserXp = 19,
        OnboardingRewardClaimed = 20
    }
}