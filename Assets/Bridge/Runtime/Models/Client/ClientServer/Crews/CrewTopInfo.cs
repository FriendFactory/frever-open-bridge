using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewTopInfo : IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int TrophyScore { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}