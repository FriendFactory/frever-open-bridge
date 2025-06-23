using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public sealed class CharacterBakedView: IMainFileContainable, ITimeChangesTrackable, IStageable
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public long? OutfitId { get; set; }
        public long ReadinessId { get; set; }
        public float HeelsHeight { get; set; }
        public string FilesInfo { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsValid { get; set; }

        public Readiness Readiness { get; set; }
        [ProtoNewField(1)] public Guid CharacterVersion { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}