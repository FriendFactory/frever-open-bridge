using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.Common
{
    public sealed class AssetOfferInfo
    {
        [ProtoNewField(1)] public long Id { get; set; }
        
        public long AssetId { get; set; }
        
        public string AssetOfferTitle { get; set; }

        public int? AssetOfferSoftCurrencyPrice { get; set; }

        public int? AssetOfferHardCurrencyPrice { get; set; }
    }
}