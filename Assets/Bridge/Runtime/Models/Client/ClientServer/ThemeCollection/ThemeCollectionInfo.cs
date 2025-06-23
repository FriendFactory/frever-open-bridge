using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.ThemeCollection
{
    public class ThemeCollectionInfo : IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool HasLargeMarketingThumbnail { get; set; }
        [ProtoNewField(1)] public long? SeasonId { get; set; }
        public List<FileInfo> Files { get; set; }
        [ProtoNewField(2)] public long UniverseId { get; set; }
    }
}