namespace Bridge.Services._7Digital.Models
{
    internal sealed class Track
    {
        public int id { get; set; }
        public string title { get; set; }
        public string version { get; set; }
        public Artist artist { get; set; }
        public int trackNumber { get; set; }
        public int duration { get; set; }
        public bool explicitContent { get; set; }
        public string isrc { get; set; }
        public string type { get; set; }
        public Release release { get; set; }
        public int discNumber { get; set; }
        public double popularity { get; set; }
        public int number { get; set; }
        public Download download { get; set; }
    }
}
