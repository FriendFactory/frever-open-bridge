using Bridge.Models.ClientServer.AssetStore;

namespace Bridge.Models.ClientServer.UserActivity
{
    internal sealed class ClaimRewardResultDto
    {
        public bool Ok { get; set; }

        public string ErrorCode { get; set; }

        public int Xp { get; set; }

        public int? SoftCurrency { get; set; }

        public int? HardCurrency { get; set; }

        public long[] SeasonRewardIds { get; set; }
        
        public AssetInfo Asset { get; set; }
        
        public LootBoxAsset[] LootBoxAssets { get; set; }
    }
}