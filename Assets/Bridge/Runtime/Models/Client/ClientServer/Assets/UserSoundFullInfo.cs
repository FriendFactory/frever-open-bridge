using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class UserSoundFullInfo: IMainFileContainable, IPlayableMusic
    {
        public long Id { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Name { get; set; }
        [ProtoNewField(1)] public int UsageCount { get; set; }
        [ProtoNewField(2)] public bool IsFavorite { get; set; }
        [ProtoNewField(3)] public GroupShortInfo Owner { get; set; }
        
        public List<FileInfo> Files { get; set; }
    }
}