using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.AssetStore
{
    public class InAppProductOffer: IThumbnailOwner
    {
        public long Id { get; set; }
        
        public string OfferKey { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string AppStoreProductRef { get; set; }

        public string PlayMarketProductRef { get; set; }

        public bool IsSeasonPass { get; set; }
        
        public List<FileInfo> Files { get; set; }

        public List<InAppProductOfferDetails> Details { get; set; } = new List<InAppProductOfferDetails>();
    }
}