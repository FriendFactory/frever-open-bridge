using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Level.Full;

namespace Bridge.Models.ClientServer.Level.Shuffle
{
    public class LevelShuffleInput
    {
        public int CharacterCount { get; set; }

        public ShuffleAssets ShuffleAssets { get; set; }

        public ICollection<InputEventInfo> Events { get; set; }
    }

    public sealed class LevelShuffleInputAI : LevelShuffleInput
    {
        public string Text { get; set; }
    }

    [Flags]
    public enum ShuffleAssets
    {
        SetLocation = 1,
        BodyAnimation = 2,
        Vfx = 4
    }

    public class InputEventInfo
    {
        public long Id { get; set; }

        public long? SetLocationId { get; set; }
        
        public long? VfxId { get; set; }

        public CharacterInputInfo[] Characters { get; set; }
    }

    public class CharacterInputInfo
    {
        public long CharacterId { get; set; }

        public long? CharacterSpawnPositionId { get; set; }

        public long? BodyAnimationId { get; set; }
    }
    
    public class LevelShuffleResult
    {
        public EventShuffleResult[] Events { get; set; }

        public SetLocationFullInfo[] SetLocations { get; set; }

        public BodyAnimationInfo[] BodyAnimations { get; set; }
        
        [ProtoNewField(1)] public VfxInfo[] Vfxs { get; set; }
    }

    public class EventShuffleResult
    {
        public long Id { get; set; }

        public long SetLocationId { get; set; }
        
        [ProtoNewField(1)] public long? VfxId { get; set; }

        public CharacterShuffleResult[] Characters { get; set; }
    }

    public class CharacterShuffleResult
    {
        public long CharacterId { get; set; }

        public long CharacterSpawnPositionId { get; set; }

        public long? CharacterSpawnPositionMovementTypeId { get; set; }

        public long? SpawnPositionGroupId { get; set; }

        public long BodyAnimationId { get; set; }

        public long? BodyAnimationMovementTypeId { get; set; }
    }
    
    public sealed class ShuffleMLResult
    {
        public bool IsReady { get; set; }

        public string Transcription { get; set; }

        public LevelShuffleResult ShuffledLevel { get; set; }
        public LevelFullData OriginalLevel { get; set; }
    }
}