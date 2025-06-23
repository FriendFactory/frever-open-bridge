using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class WardrobeSubCategory: IAssetCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? UmaSharedColorId { get; set; }
        public int SortOrder { get; set; }
        public long[] UmaAdjustments { get; set; }
        public bool HasNew { get; set; }
        [ProtoNewField(1)] public bool HasFreeAssets { get; set; }
        [ProtoNewField(2)] public bool KeepOnUndressing { get; set; }
    }
}