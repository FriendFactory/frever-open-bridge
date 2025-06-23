using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class BodyAnimationCategory: IAssetCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool HasNew { get; set; }
        [ProtoNewField(1)] public Dictionary<int, long[]> CharacterCountMovementTypes { get; set; }
    }
}