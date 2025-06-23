using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Services._7Digital.Models.TrackModels
{
    public sealed class ExternalTrackInfo : IPlayableMusic
    {
        public long Id { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public int Duration { get; set; }
        public bool ExplicitContent { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}
