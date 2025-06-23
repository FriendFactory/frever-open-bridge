using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.VideoServer
{
    public class TransformationStyle: IEntity, INamed, IThumbnailOwner, ISortOrderable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<FileInfo> Files { get; set; }
        [ProtoNewField(1)] public string DefaultPositivePrompt { get; set; }
        [ProtoNewField(2)] public string DefaultNegativePrompt { get; set; }
    }
}