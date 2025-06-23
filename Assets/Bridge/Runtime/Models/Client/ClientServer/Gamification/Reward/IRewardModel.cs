using Bridge.Models.ClientServer.Crews;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Gamification.Reward
{
    public interface IRewardModel : IThumbnailOwner
    {
        int? SoftCurrency { get; }

        int? HardCurrency { get; }

        string Title { get; }

        AssetInfo Asset { get; }
    }
    
    public static class RewardExtensions
    {
        public static RewardType GetRewardType(this IRewardModel rewardModel)
        {
            if (rewardModel.SoftCurrency != null) return RewardType.SoftCurrency;
            if (rewardModel.HardCurrency != null) return RewardType.HardCurrency;
            if (rewardModel is SeasonReward seasonReward && seasonReward.Xp != null) return RewardType.XP;
            if (rewardModel is CrewReward crewReward && crewReward.LootBox != null) return RewardType.Lootbox;
            if (rewardModel.Asset != null) return RewardType.Asset;

            return default;
        }
    }
}