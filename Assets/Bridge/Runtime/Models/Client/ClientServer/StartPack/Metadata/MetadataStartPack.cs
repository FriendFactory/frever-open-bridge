using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.CreatorScore;
using Bridge.Models.ClientServer.Onboarding;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.ThemeCollection;
using Bridge.Models.VideoServer;
using JetBrains.Annotations;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    [UsedImplicitly]
    public sealed class MetadataStartPack: IStartPack
    {
        public List<BodyAnimationCategory> BodyAnimationCategories { get; set; }
        public List<CameraAnimationType> CameraAnimationTypes { get; set; }
        public List<CameraCategory> CameraCategories { get; set; }
        public List<CharacterSpawnPositionFormationType> CharacterSpawnPositionFormationTypes { get; set; }
        public List<Gender> Genders { get; set; }
        public List<Genre> Genres { get; set; }
        public List<SetLocationCategory> SetLocationCategories { get; set; }
        public List<SpawnPositionSpaceSize> SpawnPositionSpaceSizes { get; set; }
        public List<TemplateCategory> TemplateCategories { get; set; }
        public List<UmaBundleType> UmaBundleTypes { get; set; }
        public List<UmaSharedColor> UmaSharedColors { get; set; }
        public List<UnityAssetType> UnityAssetTypes { get; set; }
        public List<VfxCategory> VfxCategories { get; set; }
        public List<VoiceFilterFullInfo> VoiceFilters { get; set; }
        public List<WardrobeCategory> WardrobeCategories { get; set; }
        public List<WardrobeCategoryType> WardrobeCategoryTypes { get; set; }
        public List<UmaBundleFullInfo> GlobalUmaBundles { get; set; }
        public List<UmaAdjustment> UmaAdjustments { get; set; }
        public List<CameraFilterCategory> CameraFilterCategories { get; set; }
        public List<VoiceFilterCategory> VoiceFilterCategories { get; set; }
        public List<CreatorBadge> CreatorBadges { get; set; }
        public List<MovementType> MovementTypes { get; set; }
        public List<OnboardingQuestGroup> OnboardingQuests { get; set; }
        public int UnlockCreateTemplateFromVideoOnLevel { get; set; }
        [ProtoNewField(1)] public int UnlockVideoToFeedOnLevel { get; set; }
        [ProtoNewField(2)] public int UnlockCrewCreationOnLevel { get; set; }
        [ProtoNewField(3)] public List<ThemeCollectionInfo> ThemeCollections { get; set; }
        [ProtoNewField(4)] public Dictionary<RestrictionReason, string> TemplateFromVideoRestrictionReasons { get; set; }
        [ProtoNewField(5)] public CaptionMetadata CaptionMetadata { get; set; }
        [ProtoNewField(6)] public EditorMetadata EditorMetadata { get; set; }
        [ProtoNewField(7)] public IntellectualProperty[] IntellectualProperty { get; set; }
        [ProtoNewField(8)] public Universe[] Universes { get; set; }
        [ProtoNewField(9)] public FeatureFlag[] FeatureFlags { get; set; }
    }
}