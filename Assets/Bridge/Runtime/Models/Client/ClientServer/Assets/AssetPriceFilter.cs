namespace Bridge.Models.ClientServer.Assets
{
    public enum AssetPriceFilter
    {
        None = 0,
        Free = 1,
        WithSoftCurrency = 2,
        WithHardCurrency = 3,
        WithFreeAndSoftCurrency = 4,
        WithFreeAndHardCurrency = 5,
        WithSoftCurrencyAndHardCurrency = 6
    }
}