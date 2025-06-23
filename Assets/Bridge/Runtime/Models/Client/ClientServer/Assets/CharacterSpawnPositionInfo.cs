using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CharacterSpawnPositionInfo: IThumbnailOwner, INamed, IUnityGuid
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long SpawnPositionSpaceSizeId { get; set; }
        public Guid UnityGuid { get; set; }
        public bool IsDefault { get; set; }
        public float[] Position { get; set; }
        public float[] Rotation { get; set; }
        public long? MovementTypeId { get; set; }
        public bool AvailableForSelection { get; set; }
        public int? SpawnPositionGroupId { get; set; }
        public int? SpawnOrderIndex { get; set; }
        public long? DefaultBodyAnimationId { get; set; }
        public bool SupportFormation { get; set; }
        [ProtoNewField(1)] public bool AllowUsingSubSpawnPositions { get; set; }
        [ProtoNewField(2)] public long[] SecondaryMovementTypeIds { get; set; }
        [ProtoNewField(3)] public long[] KeepAnimationCategoryIds { get; set; }
        [ProtoNewField(4)] public long[] CompatibleRaceIds { get; set; }
        [ProtoNewField(5)] public SpawnPositionAdjustment[] Adjustments { get; set; }

        public List<FileInfo> Files { get; set; }
        public List<LightSettingsInfo> LightSettings { get; set; }
    }

    public sealed class SpawnPositionAdjustment
    {
        public long[] GenderIds { get; set; }
        public float Scale { get; set; }
        public float AdjustY { get; set; }
        [ProtoNewField(1)] public float AdjustX { get; set; }
        [ProtoNewField(2)] public float AdjustZ { get; set; }
    }
}