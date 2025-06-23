namespace Bridge.Models.AsseManager
{
    public partial class TemplateSubCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long SortingOrder { get; set; }
        public long TemplateCategoryId { get; set; }
    }
}