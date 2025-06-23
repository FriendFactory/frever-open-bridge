using System.Collections.Generic;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.CreatorScore
{
    public class CreatorBadge
    {
        public int Level { get; set; }

        public string Title { get; set; }

        public int CreatorScoreRequired { get; set; }
        
        public string Milestone { get; set; }

        public CreatorBadgeReward[] Rewards { get; set; }
    }
    
    public class CreatorBadgeReward: IThumbnailOwner
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int? SoftCurrency { get; set; }

        public int? HardCurrency { get; set; }

        public AssetInfo Asset { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}