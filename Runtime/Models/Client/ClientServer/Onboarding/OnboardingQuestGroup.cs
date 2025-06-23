using System.Collections.Generic;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Onboarding
{
    public class OnboardingQuestGroup
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<FileInfo> Files { get; set; }
        public List<OnboardingQuest> Quests { get; set; }
        public List<OnboardingReward> Rewards { get; set; }
    }

    public class OnboardingQuest
    {
        public long Id { get; set; }
        public string QuestType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? QuestParameter { get; set; }
    }

    public class OnboardingReward
    {
        public long Id { get; set; }

        public int? SoftCurrency { get; set; }

        public int? HardCurrency { get; set; }

        public int? Xp { get; set; }

        public string Title { get; set; }

        public AssetInfo Asset { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}