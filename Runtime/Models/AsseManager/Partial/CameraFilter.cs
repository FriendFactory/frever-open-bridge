using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class CameraFilter: IEntity, IGroupAccessible, INamed, IThumbnailOwner, 
        ITaggable, IStageable, ITimeChangesTrackable, ISortOrderable, IStartPackIncludable, ICategoryMember
    {
        public List<FileInfo> Files { get; set; }
        public long CategoryId => CameraFilterCategoryId;
        public ICategory Category => CameraFilterCategory;
    }
}