using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class WardrobeCategory: IThumbnailOwner, IAssetCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long WardrobeCategoryTypeId { get; set; }
        public int SortOrder { get; set; }
        public List<FileInfo> Files { get; set; }
        public bool HasSubCategoryAll { get; set; }
        public bool HasNew { get; set; }
        public List<WardrobeSubCategory> SubCategories { get; set; }
    }
}