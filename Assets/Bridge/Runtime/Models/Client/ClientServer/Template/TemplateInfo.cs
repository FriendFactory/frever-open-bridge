using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Template
{
    public sealed class TemplateInfo : IThumbnailOwner
    {
        public long Id { get; set; }

        public List<FileInfo> Files { get; set; }

        public long TemplateSubCategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CharacterCount { get; set; }

        public string ArtistName { get; set; }

        public string SongName { get; set; }

        public bool ReverseThumbnail { get; set; }

        public long[] Tags { get; set; }

        public int UsageCount { get; set; }

        public long CreatorId { get; set; }

        public long OriginalVideoId { get; set; }

        public TemplateGroupInfo Creator { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }
    }
}