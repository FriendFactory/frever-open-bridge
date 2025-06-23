using System.Collections.Generic;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.UserActivity
{
    public class LootBoxAsset : IThumbnailOwner
    {
        public long Id { get; set; }
        public long AssetTierId { get; set; }
        public AssetStoreAssetType AssetType { get; set; }
        public int? SoftCurrency { get; set; }
        public int? HardCurrency { get; set; }
        public List<FileInfo> Files { get; set; }

        public AssetInfo Asset => new AssetInfo
        {
            Id = Id,
            AssetType = AssetType,
            Files = Files
        };
    }
}