using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class CameraAnimationType: IEntity, IThumbnailOwner, IFilesAttachedEntity
    {
        public List<FileInfo> Files { get; set; }
    }
}