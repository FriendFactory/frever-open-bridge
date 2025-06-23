using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class IntellectualProperty
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Race[] Races { get; set; }
        
        [ProtoNewField(1)] public Watermark Watermark { get; set; }
    }

    public class Race
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Prefab { get; set; }

        public Gender[] Genders { get; set; }
    }

    public class Universe: IThumbnailOwner, ISortOrderable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<FileInfo> Files { get; set; }

        public UniverseRace[] Races { get; set; }
        
        [ProtoNewField(1)] public bool IsNew { get; set; }
        
        [ProtoNewField(2)] public int SortOrder { get; set; }
        
        [ProtoNewField(3)] public bool AllowStartGift { get; set; }
        
        [ProtoNewField(4)] public bool AllAssetsFree { get; set; }
    }

    public class UniverseRace
    {
        public long RaceId { get; set; }

        public UniverseAndRaceSettings Settings { get; set; }
    }
    
    public class UniverseAndRaceSettings
    {
        public bool CanUseCharacters { get; set; }

        public bool CanRemixVideos { get; set; }
        
        [ProtoNewField(1)] public bool SupportsSelfieToAvatar { get; set; }
        [ProtoNewField(2)] public bool CanCreateCharacters { get; set; }
    }

    public class Watermark : IMainFileContainable, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int DurationSeconds { get; set; }
        public List<FileInfo> Files { get; set; }
        [ProtoNewField(1)] public PositionSettings[] Positions { get; set; }
    }
    
    public class PositionSettings
    {
        public VideoOrientation VideoOrientation { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float Scale { get; set; }
    }
    
    public enum VideoOrientation
    {
        Portrait = 1,
        Landscape = 2
    }
}