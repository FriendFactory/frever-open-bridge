using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Gamification
{
    public sealed class CurrentSeason
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? PremiumPrice { get; set; }

        public List<MarketingScreenshot> MarketingScreenshots { get; set; }

        public SeasonLevel[] Levels { get; set; } = { };

        public SeasonQuest[] Quests { get; set; } = { };
        
        [ProtoNewField(1)] public int LevelHardCurrencyPrice { get; set; }
    }

    public sealed class MarketingScreenshot: IThumbnailOwner
    {
        public long Id { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}