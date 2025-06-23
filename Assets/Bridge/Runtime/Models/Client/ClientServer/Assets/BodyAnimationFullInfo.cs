using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class BodyAnimationFullInfo: INamed, ISortOrderable, IThumbnailOwner, IMainFileContainable, IPurchasable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<FileInfo> Files { get; set; }

        public bool Locomotion { get; set; }

        public bool Looping { get; set; }

        public bool Continous { get; set; }

        public long BodyAnimationCategoryId { get; set; }

        public long? BodyAnimationSpaceSizeId { get; set; }

        public long[] Tags { get; set; }

        public int SortOrder { get; set; }

        public bool HasFaceAnimation { get; set; }

        public long? MovementTypeId { get; set; }

        public long? BodyAnimationGroupId { get; set; }

        public int? OrderIndexInGroup { get; set; }

        public AssetOfferInfo AssetOffer { get; set; }
        
        [ProtoNewField(1)] public BodyAnimationAndVfxDto BodyAnimationAndVfx { get; set; }
    }
}