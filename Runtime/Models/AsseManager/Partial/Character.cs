using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class Character: IEntity, IGroupAccessible, IThumbnailOwner, ITimeChangesTrackable, INamed, ISortOrderable, IStageable
    {
        public List<FileInfo> Files { get; set; }
    }
}