using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.VideoServer
{
    public sealed class TemplateChallenge: IEventTemplate, IThumbnailOwner
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long ChallengeSortOrder { get; set; }

        public int UsageCount { get; set; }
        
        public long TemplateCategoryId { get; set; }

        public Video[] Videos { get; set; }

        public List<FileInfo> Files { get; set; }
        
        public HashtagInfo HashtagInfo { get; set; } 
    }
}