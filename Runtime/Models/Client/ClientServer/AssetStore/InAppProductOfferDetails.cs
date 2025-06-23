using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.AssetStore
{
    public class InAppProductOfferDetails: IThumbnailOwner
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AssetInfo Asset { get; set; }
        public int? HardCurrency { get; set; }
        public int? SoftCurrency { get; set; }
        public List<FileInfo> Files { get; set; }
    }

    public class AssetInfo : IThumbnailOwner
    {
        public long Id { get; set; }

        public AssetStoreAssetType AssetType { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}