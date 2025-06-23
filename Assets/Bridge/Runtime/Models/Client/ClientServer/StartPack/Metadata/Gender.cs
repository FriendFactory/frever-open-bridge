using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class Gender : IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [ProtoNewField(1)] public string Identifier { get; set; }
        [ProtoNewField(2)] public string UpperUnderwearOverlay { get; set; }
        [ProtoNewField(3)] public string LowerUnderwearOverlay { get; set; }
        [ProtoNewField(4)] public string UmaRaceKey { get; set; }
        [ProtoNewField(5)] public bool CanCreateCharacter { get; set; }
    }
}