namespace Bridge.Services._7Digital.QueryModels
{
    public sealed class PlaylistData
    {
        public string name { get; set; }
        public string status { get; set; } = "published";
        public TrackData[] tracks { get; set; }
    }
}
