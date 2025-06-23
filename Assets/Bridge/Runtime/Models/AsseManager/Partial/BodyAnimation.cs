using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class BodyAnimation : INamed, IStageableAsset, IThumbnailOwner, ICategoryMember,
        IMainFileContainable, ITaggable, ISortOrderable, IStartPackIncludable, ISizeStorable
    {
        public long CategoryId => BodyAnimationCategoryId;
        public ICategory Category => BodyAnimationCategory;

        public List<FileInfo> Files { get; set; }
    }
}