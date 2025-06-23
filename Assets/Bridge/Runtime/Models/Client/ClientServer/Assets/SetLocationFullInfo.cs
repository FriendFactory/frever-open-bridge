using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class SetLocationFullInfo: IThumbnailOwner, INamed, INewTrackable, IPurchasable, IMinLevelRequirable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool AllowPhoto { get; set; }
        [ProtoNewField(1)]
        public bool PhotoAutoPick { get; set; }
        public bool AllowVideo { get; set; }
        public long CategoryId { get; set; }
        public int? SortOrderByCategory { get; set; }
        public bool IsNew { get; set; }
        public long? SeasonLevel { get; set; }
        
        public List<FileInfo> Files { get; set; }
        public SetLocationBundleInfo SetLocationBundle { get; set; }
        public AssetOfferInfo AssetOffer { get; set; }
        public List<CharacterSpawnPositionInfo> CharacterSpawnPosition { get; set; }
        public AssetTierInfo AssetTier { get; set; }
    }
}