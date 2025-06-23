namespace Bridge.Models.ClientServer.AssetStore
{
    public class AvailableOffers
    {
        public CurrencyExchangeOffer[] CurrencyExchange { get; set; }
        public InAppProductSlot[] InAppProducts { get; set; }
        
        public InAppProductOffer[] HardCurrencyOffers { get; set; }
    }

    public class InAppProductSlot
    {
        public InAppProductSlotState State { get; set; }

        /// <summary>
        ///     Might be null if slot is empty
        /// </summary>
        public InAppProductOffer Offer { get; set; }
    }

    public enum InAppProductSlotState
    {
        // Offer might be purchased
        Available = 1,

        // Empty Slot without Offer
        Empty = 2,

        // Offer that User already purchased
        SoldOut = 3,

        // Offer which can't be purchased (might be needed in future to advertise Premium pass for example)
        Unavailable = 4
    }
}