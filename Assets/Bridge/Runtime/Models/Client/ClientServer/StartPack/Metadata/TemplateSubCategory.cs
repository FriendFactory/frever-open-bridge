using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class TemplateSubCategory: ICategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}