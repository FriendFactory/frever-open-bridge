namespace Bridge.Services._7Digital.Models.PlaylistModels
{
    internal sealed class PlaylistTrack
    {
        public string playlistItemId { get; set; }
        public string trackId { get; set; }
        public string trackTitle { get; set; }
        public string trackVersion { get; set; }
        public string artistAppearsAs { get; set; }
        public string releaseId { get; set; }
        public string releaseTitle { get; set; }
        public string releaseArtistAppearsAs { get; set; }
        public string releaseVersion { get; set; }
        public string source { get; set; }
        public string audioUrl { get; set; }
        public string user { get; set; }
    }
}
