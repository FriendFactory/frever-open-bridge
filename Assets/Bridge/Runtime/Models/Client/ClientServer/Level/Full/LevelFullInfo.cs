using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class LevelFullInfo
    {
        public long Id { get; set; }

        public long? RemixedFromLevelId { get; set; }

        public long? SchoolTaskId { get; set; }

        public string Description { get; set; }

        public bool ContainsCopyrightedContent { get; set; }
        
        [ProtoNewField(1)] public long LevelTypeId { get; set; } 

        public List<EventFullInfo> Event { get; set; }
    }
}