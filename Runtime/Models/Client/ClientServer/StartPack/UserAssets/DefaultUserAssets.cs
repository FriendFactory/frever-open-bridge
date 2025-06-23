using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.StartPack.Prefetch;

namespace Bridge.Models.ClientServer.StartPack.UserAssets
{
    public sealed class DefaultUserAssets: IStartPack
    {
        public long? MainCharacterId { get; set; }

        public List<CharacterInfo> UserCharacters { get; set; }

        public long TemplateId { get; set; }
        
        public long TemplateEventId { get; set; }
        
        public bool HasInvitationReward { get; set; }

        public PurchasedAssetsData PurchasedAssetsData { get; set; }
        
        [ProtoNewField(1)] public long OnboardingTemplateId { get; set; }
        
        [ProtoNewField(2)] public long[] MainCharacterIds { get; set; }
    }
    
    public sealed class PurchasedAssetsData
    {
        public long[] Wardrobes { get; set; }
        public long[] SetLocations { get; set; }
        public long[] CameraFilters { get; set; }
        public long[] Vfxs { get; set; }
        public long[] VoiceFilters { get; set; }
        public long[] BodyAnimations { get; set; }
    }
}