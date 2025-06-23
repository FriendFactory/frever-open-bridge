using System;

namespace Bridge.Models.AsseManager
{
    public partial class Template
    {
        public long Id { get; set; }
        public string FilesInfo { get; set; }
        public long TemplateSubCategoryId { get; set; }
        public string Title { get; set; }
        public long CreatorId { get; set; }
        public long EventId { get; set; }
        public long? TopListPositionInDiscovery { get; set; }
        public long? TrendingSortingOrder { get; set; }
        public long? PromotionalSortingOrder { get; set; }
        public long? CategorySortingOrder { get; set; }
        public long? SubCategorySortingOrder { get; set; }
        public int CharacterCount { get; set; }
        public string SongName { get; set; }
        public bool IsDeleted { get; set; }
        public bool ReverseThumbnail { get; set; }
        public string ArtistName { get; set; }
        public long ReadinessId { get; set; }
        public string Description { get; set; }
        public long? OnBoardingSortingOrder { get; set; }
        public long? ChallengeSortOrder { get; set; }

        public Readiness Readiness { get; set; }
        public long[] Tags { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
