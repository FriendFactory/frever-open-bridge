using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Gamification
{
    public class LootBox : IThumbnailOwner
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}