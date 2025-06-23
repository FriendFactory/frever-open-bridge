using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class Vfx : IStageableAsset, IThumbnailOwner, INamed, ICategoryMember, IMainFileContainable,
        ITaggable, ISortOrderable, IStartPackIncludable, ISizeStorable
    {
        public long CategoryId => VfxCategoryId;
        public ICategory Category => VfxCategory;

        public List<FileInfo> Files { get; set; }
    }
}