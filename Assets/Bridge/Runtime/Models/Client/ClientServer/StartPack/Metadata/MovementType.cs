using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class MovementType: IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool SupportFormation { get; set; }
        public bool DependsOnCharacterHeelsHeight { get; set; }
        [ProtoNewField(1)] public List<FileInfo> Files { get; set; }
    }
}