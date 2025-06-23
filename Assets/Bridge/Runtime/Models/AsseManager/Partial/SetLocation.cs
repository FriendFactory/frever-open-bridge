using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class SetLocation: INamed, IStageableAsset, IThumbnailOwner, ICategoryMember, ISetLocationBundleChild, ITaggable, IStartPackIncludable
    {
        public long CategoryId => SetLocationCategoryId;
        public ICategory Category => SetLocationCategory;
        public List<FileInfo> Files { get; set; }
    }
}