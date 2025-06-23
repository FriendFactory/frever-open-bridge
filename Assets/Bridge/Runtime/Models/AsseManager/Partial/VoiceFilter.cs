using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class VoiceFilter : IGroupAccessible, INamed, IStageable, ICategoryMember, IThumbnailOwner, ISortOrderable
    {
        public long CategoryId => VoiceFilterCategoryId;
        public ICategory Category => VoiceFilterCategory;

        public List<FileInfo> Files { get; set; }
    }
}
