namespace Bridge.Services._7Digital.Models.TrackModels
{
    internal sealed class BatchTracksDetails
    {
        public BatchItems Items { get; set; }
    }
    
    internal sealed class BatchItems
    {
        public Track[] Tracks { get; set; }
        public TrackError[] Errors { get; set; }
    }
}