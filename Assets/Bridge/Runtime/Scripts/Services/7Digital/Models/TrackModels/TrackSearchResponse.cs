namespace Bridge.Services._7Digital.Models
{
    internal sealed class TrackSearchResponse
    {
        public string status { get; set; }
        public string version { get; set; }
        public SearchResults searchResults { get; set; }
    }
}
