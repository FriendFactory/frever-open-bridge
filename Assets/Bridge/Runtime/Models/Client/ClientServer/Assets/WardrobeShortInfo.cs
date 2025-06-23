using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class WardrobeShortInfo: IThumbnailOwner, INamed, INewTrackable, IPurchasable, IMinLevelRequirable, ICategorizable, ISubCategorizable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long WardrobeCategoryId { get; set; }
        public long[] WardrobeSubCategoryIds { get; set; }
        public bool IsNew { get; set; }
        public long? SeasonLevel { get; set; }

        public AssetOfferInfo AssetOffer { get; set; }
        public AssetTierInfo AssetTier { get; set; }
        public List<FileInfo> Files { get; set; }
        
        public long CategoryId => WardrobeCategoryId;
        public long[] SubCategories => WardrobeSubCategoryIds;
    }
}