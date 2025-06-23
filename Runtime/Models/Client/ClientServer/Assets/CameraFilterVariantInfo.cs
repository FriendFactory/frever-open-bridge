using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CameraFilterVariantInfo: IMainFileContainable, IThumbnailOwner, INamed, ISortOrderable, INewTrackable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<FileInfo> Files { get; set; }
        public bool IsNew { get; set; }
        public long CameraFilterId { get; set; }
    }
}