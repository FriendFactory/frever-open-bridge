using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class EditorMetadata
    {
        public BodyAnimationInfo CharacterEditingAnimation { get; set; }
        public BodyAnimationInfo CharacterPreviewAnimation { get; set; }
        public BodyAnimationInfo[] BackgroundCharacterAnimations { get; set; }
        [ProtoNewField(1)] public CharacterSpawnPositionFormation[] CharacterEditingSpawnFormations { get; set; }
    }
}