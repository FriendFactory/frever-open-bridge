using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.ClientServer.AssetStore;

namespace Bridge.ClientServer.InAppPurchases
{
    public class AssetPurchaseResult: IOkResponse
    {
        public bool Ok { get; set; }

        public string ErrorMessage { get; set; }

        public AssetPurchaseErrorCode ErrorCode { get; set; }

        public AssetPurchase Purchase { get; set; }
    }

    public enum AssetPurchaseErrorCode
    {
        NotEnoughCurrency,
        AssetNotAvailableForPurchase,
        ErrorSavingToDb
    }
    
    public class AssetPurchase
    {
        public long AssetTransactionGroupId { get; set; }

        public long GroupId { get; set; }

        public DateTime CreatedTime { get; set; }

        public AssetStoreTransactionType TransactionType { get; set; }

        public int SoftCurrencyTotal => Rows.Count == 0 ? 0 : Rows.Sum(r => r.SoftCurrencyAmount);

        public int HardCurrencyTotal => Rows.Count == 0 ? 0 : Rows.Sum(r => r.HardCurrencyAmount);

        public List<AssetPurchaseRow> Rows { get; set; }
    }

    public class AssetPurchaseRow
    {
        public int SoftCurrencyAmount { get; set; }

        public int HardCurrencyAmount { get; set; }

        public long AssetOfferId { get; set; }

        public long AssetId { get; set; }

        public AssetStoreAssetType AssetType { get; set; }
    }
}