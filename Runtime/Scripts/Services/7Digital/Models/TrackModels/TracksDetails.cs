namespace Bridge.Services._7Digital.Models.TrackModels
{
    internal sealed class TracksDetails
    {
        public Items Items { get; set; }
    }

    internal sealed class Items
    {
        public Track[] Tracks { get; set; }
    }
}
