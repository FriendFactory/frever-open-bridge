using System;
using Bridge.Models.ClientServer.UserActivity;
using Bridge.Models.ClientServer.AssetStore;

namespace Bridge.Results
{
    public sealed class ClaimRewardResult : Result
    {
        private readonly ClaimRewardResultDto _rewardResultDto;

        public int Xp => _rewardResultDto.Xp;

        public int? SoftCurrency => _rewardResultDto.SoftCurrency;

        public int? HardCurrency => _rewardResultDto.HardCurrency;
        
        public long[] SeasonRewardIds => _rewardResultDto.SeasonRewardIds;
        
        public AssetInfo Asset => _rewardResultDto.Asset;

        public LootBoxAsset[] LootBoxAssets => _rewardResultDto.LootBoxAssets;

        private ClaimRewardResult(ClaimRewardResultDto claimResultDto)
        {
            _rewardResultDto = claimResultDto;
        }

        private ClaimRewardResult(string error): base(error)
        {
        }
        
        internal static ClaimRewardResult Success(ClaimRewardResultDto claimResultDto)
        {
            return new ClaimRewardResult(claimResultDto);
        }

        internal static ClaimRewardResult Error(string message)
        {
            return new ClaimRewardResult(message);
        }
    }
}