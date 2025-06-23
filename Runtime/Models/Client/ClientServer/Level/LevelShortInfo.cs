using System;
using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Level
{
    public class LevelShortInfo
    {
        public long Id { get; set; }

        public long FirstEventId { get; set; }

        public DateTime CreatedTime { get; set; }

        public List<FileInfo> FirstEventFiles { get; set; }
    }
}