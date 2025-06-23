using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using ProtoBuf;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class CameraAnimationTemplate: IThumbnailOwner, ISortOrderable, INamed
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public long CameraAnimationTypeId { get; set; }
        public long? Size { get; set; }
        public int SortOrder { get; set; }
        public List<FileInfo> Files { get; set; }

        [ProtoIgnore]
        public string Name
        {
            get => DisplayName;
            set => DisplayName = value;
        }
    }
}