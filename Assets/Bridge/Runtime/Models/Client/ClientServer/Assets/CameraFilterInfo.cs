using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CameraFilterInfo: IThumbnailOwner, INamed, INewTrackable, IPurchasable, IMinLevelRequirable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CameraFilterCategoryId { get; set; }
        public bool IsNew { get; set; }
        public long? SeasonLevel { get; set; }
        
        public List<FileInfo> Files { get; set; }

        public AssetOfferInfo AssetOffer { get; set; }
        public AssetTierInfo AssetTier { get; set; }

        public List<CameraFilterVariantInfo> CameraFilterVariants { get; set; }
    }
}