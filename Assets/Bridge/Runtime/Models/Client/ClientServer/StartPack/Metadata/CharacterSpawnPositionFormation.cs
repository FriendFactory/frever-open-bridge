using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class CharacterSpawnPositionFormation: IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int CharacterCount { get; set; }
        public int SortOrder { get; set; }
        public bool SupportsMultiCharacterAnimation { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}