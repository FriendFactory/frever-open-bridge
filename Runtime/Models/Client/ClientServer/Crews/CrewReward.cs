using System.Collections.Generic;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.ClientServer.Gamification;
using Bridge.Models.Common.Files;
using Bridge.Models.ClientServer.Gamification.Reward;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewReward : IRewardModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int RequiredTrophyScore { get; set; }
        public int? SoftCurrency { get; set; }
        public int? HardCurrency { get; set; }
        public LootBox LootBox { get; set; }
        public AssetInfo Asset { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}