using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class CameraAnimationTemplate: IThumbnailOwner, ISortOrderable, ICategoryMember
    {
        public List<FileInfo> Files { get; set; }
        public long CategoryId => CameraCategoryId;
        public ICategory Category => CameraCategory;
    }
}