using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class CameraFilterVariant : INamed, IMainFileContainable, IThumbnailOwner, ITimeChangesTrackable,
        IStageable, ISortOrderable, ISizeStorable
    {
        public List<FileInfo> Files { get; set; }
    }
}