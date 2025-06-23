using System.Collections.Generic;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common.Files;
using Bridge.Models.ClientServer.Gamification.Reward;

namespace Bridge.Models.ClientServer.Gamification
{
    public class SeasonReward: IRewardModel
    {
        public long Id { get; set; }

        public long SeasonId { get; set; }

        public int? Level { get; set; }

        public long? SeasonQuestId { get; set; }

        public bool IsPremium { get; set; }

        public int? SoftCurrency { get; set; }

        public int? HardCurrency { get; set; }

        public int? Xp { get; set; }

        public string Title { get; set; }

        public AssetInfo Asset { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}