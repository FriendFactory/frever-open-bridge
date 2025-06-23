using System;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.AssetStore
{
    public sealed class Deal
    {
        public long AssetOfferId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DealMarketingScreenshot[] MarketingScreenshots { get; set; }

        public DateTime? PublicationDate { get; set; }

        public DateTime? DepublicationDate { get; set; }

        public OutfitFullInfo Outfit { get; set; }

        public int? SoftCurrencyPriceNoDiscount { get; set; }
        public int? HardCurrencyPriceNoDiscount { get; set; }
        public int? SoftCurrencyPriceWithDiscount { get; set; }
        public int? HardCurrencyPriceWithDiscount { get; set; }
        public int? YourSoftCurrencyPrice { get; set; }
        public int? YourHardCurrencyPrice { get; set; }

        public int DiscountPercent { get; set; }

        public DealAsset[] Assets { get; set; }

        public bool IsOwned { get; set; }
        [ProtoNewField(1)] public bool IsPurchaseLevel { get; set; }
    }

    public class DealAsset
    {
        public long AssetId { get; set; }
        public AssetStoreAssetType AssetType { get; set; }
        public FileInfo[] Files { get; set; }
        public string Title { get; set; }

        public bool IsOwned { get; set; }
        public int? SoftCurrencyPriceNoDiscount { get; set; }
        public int? HardCurrencyPriceNoDiscount { get; set; }
        public int? SoftCurrencyPriceWithDiscount { get; set; }
        public int? HardCurrencyPriceWithDiscount { get; set; }
    }

    public class DealMarketingScreenshot
    {
        public FileInfo[] Files { get; set; }
    }
}