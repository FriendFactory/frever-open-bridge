using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class VfxInfo: IThumbnailOwner, INamed, IMainFileContainable, INewTrackable, IPurchasable, IMinLevelRequirable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Looping { get; set; }
        public long VfxCategoryId { get; set; }
        public bool IsNew { get; set; }
        public long? SeasonLevel { get; set; }
        
        public AssetTierInfo AssetTier { get; set; }
        public AssetOfferInfo AssetOffer { get; set; }
        [ProtoNewField(1)] public VfxAdjustment[] Adjustments { get; set; }
        [ProtoNewField(2)] public string AnchorPoint { get; set; }
        [ProtoNewField(3)] public bool FollowRotation { get; set; }
        [ProtoNewField(4)] public BodyAnimationAndVfxDto BodyAnimationAndVfx { get; set; }

        public List<FileInfo> Files { get; set; }
    }

    public sealed class VfxAdjustment
    {
        public long[] GenderIds { get; set; }
        public Vector3Dto AdjustPosition { get; set; }
        public Vector3Dto AdjustRotation { get; set; }
        [ProtoNewField(1)] public float? Scale { get; set; }
        [ProtoNewField(2)] public Space? Space { get; set; }
    }

    public enum Space
    {
        Local,
        World
    }
}