namespace Bridge.Services._7Digital.Models
{
    internal sealed class SearchResult
    {
        public string type { get; set; }
        public double score { get; set; }
        public Track track { get; set; }
    }
}
