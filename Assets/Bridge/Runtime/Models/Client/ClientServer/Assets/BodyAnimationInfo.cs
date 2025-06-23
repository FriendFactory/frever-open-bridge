using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class BodyAnimationInfo: INamed, IThumbnailOwner, IMainFileContainable, INewTrackable, IPurchasable, IMinLevelRequirable
    {
        public long Id { get; set; }

        public bool IsNew { get; set; }

        public string Name { get; set; }

        public bool Continous { get; set; }

        public bool HasFaceAnimation { get; set; }

        public long BodyAnimationCategoryId { get; set; }

        public List<FileInfo> Files { get; set; }

        public bool Locomotion { get; set; }

        public bool Looping { get; set; }

        public long? SeasonLevel { get; set; }

        public long? MovementTypeId { get; set; }

        public long? BodyAnimationGroupId { get; set; }

        public int? OrderIndexInGroup { get; set; }

        public AssetOfferInfo AssetOffer { get; set; }

        public AssetTierInfo AssetTier { get; set; }
        
        [ProtoNewField(1)] public BodyAnimationAndVfxDto BodyAnimationAndVfx { get; set; }
    }
}