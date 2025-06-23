using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class VfxFullInfo: IMainFileContainable, IThumbnailOwner, INamed
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<FileInfo> Files { get; set; }

        public long[] Tags { get; set; }

        public long VfxCategoryId { get; set; }

        public long VfxTypeId { get; set; }

        public long VfxDirectionId { get; set; }

        public long VfxWorldSizeId { get; set; }

        public bool Looping { get; set; }

        public long SizeKb { get; set; }

        public int? PolyCount { get; set; }

        public int? Duration { get; set; }

        public int SortOrder { get; set; }
        
        public AssetOfferInfo AssetOffer { get; set; }
        
        [ProtoNewField(1)] public string AnchorPoint { get; set; }
       
        [ProtoNewField(2)] public bool FollowRotation { get; set; }
        
        [ProtoNewField(3)] public VfxAdjustment[] Adjustments { get; set; }
        
        [ProtoNewField(4)] public BodyAnimationAndVfxDto BodyAnimationAndVfx { get; set; }
    }
}