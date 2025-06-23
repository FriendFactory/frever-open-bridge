using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using System;

namespace Bridge.Models.ClientServer.Assets
{
    public sealed class WardrobeFullInfo: IThumbnailOwner, INamed, IPurchasable, IMinLevelRequirable, ICategorizable, ISubCategorizable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long GenderId { get; set; }

        public long? WardrobeGenderGroupId { get; set; }

        public List<FileInfo> Files { get; set; }

        public bool OverridesUpperUnderwear { get; set; }

        public bool OverridesLowerUnderwear { get; set; }

        public long UmaBundleId { get; set; }
        
        public UmaBundleFullInfo UmaBundle { get; set; }
        
        public long WardrobeCategoryId { get; set; }
        public long[] WardrobeSubCategoryIds { get; set; }
        public AssetOfferInfo AssetOffer { get; set; }
        public long? SeasonLevel { get; set; }
        public AssetTierInfo AssetTier { get; set; }
        [ProtoNewField(1)] public long[] CompatibleGenderIds { get; set; }
        [ProtoNewField(2)] public PhysicsSettings PhysicsSettings { get; set; }

        public long CategoryId => WardrobeCategoryId;
        public long[] SubCategories => WardrobeSubCategoryIds;
    }

    public sealed class UmaBundleFullInfo: IMainFileContainable, INamed
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string AssetBundleHash { get; set; }

        public long UmaBundleTypeId { get; set; }

        public List<FileInfo> Files { get; set; }

        public List<UmaAssetInfo> UmaAssets { get; set; }
        
        public List<UmaBundleFullInfo> DependentUmaBundles { get; set; } = new();
     
        [ProtoNewField(1)] public long[] GenderIds { get; set; }
    }

    public sealed class UmaAssetInfo: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Hash { get; set; }
        public long? SlotId { get; set; }
        public string SlotName { get; set; }
        public List<UmaAssetFileInfo> UmaAssetFiles { get; set; }
    }

    public class UmaAssetFileInfo: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<long> UnityAssetTypesIds { get; set; }
    }
    
    [Serializable]
    public class PhysicsSettings
    {
        public float Damping { get; set; }
        [ProtoNewField(1)] public AnimationCurveDto DampingDistrib { get; set; }
        public float Elasticity { get; set; }
        [ProtoNewField(2)] public AnimationCurveDto ElasticityDistrib { get; set; }
        public float Stiffness { get; set; }
        [ProtoNewField(3)] public AnimationCurveDto StiffnessDistrib { get; set; }
        public float Inert { get; set; }
        [ProtoNewField(4)] public AnimationCurveDto InertDistrib { get; set; }
        public float Radius { get; set; }
        [ProtoNewField(5)] public AnimationCurveDto RadiusDistrib { get; set; }
        public Vector3Dto EndOffset { get; set; }
        public Vector3Dto Gravity { get; set; }
        public Vector3Dto Force { get; set; }
    }
}