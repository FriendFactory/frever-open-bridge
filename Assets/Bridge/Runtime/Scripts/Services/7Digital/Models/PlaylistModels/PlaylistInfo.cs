using Bridge.Models.Common;
using Bridge.Services._7Digital.Models.TrackModels;

namespace Bridge.Services._7Digital.Models.PlaylistModels
{
    public sealed class PlaylistInfo
    {
        public string Title { get; set; }
        public ExternalTrackInfo[] Tracks { get; set; }
    }
}
