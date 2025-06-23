using System.Linq;

namespace Bridge.Models.ClientServer.Gamification
{
    public class ClaimPastRewardsResult
    {
        public int RewardCount { get; set; }

        public int SoftCurrency { get; set; }

        public int HardCurrency { get; set; }

        public PastSeasonAssetReward[] Assets { get; set; } = { };

        public static ClaimPastRewardsResult operator +(ClaimPastRewardsResult r1, ClaimPastRewardsResult r2)
        {
            return new ClaimPastRewardsResult
                   {
                       HardCurrency = r1.HardCurrency + r2.HardCurrency,
                       SoftCurrency = r1.SoftCurrency + r2.SoftCurrency,
                       RewardCount = r1.RewardCount + r2.RewardCount,
                       Assets = r1.Assets.Concat(r2.Assets).GroupBy(a => a.Id).Select(g => g.First()).ToArray()
                   };
        }
    }
}