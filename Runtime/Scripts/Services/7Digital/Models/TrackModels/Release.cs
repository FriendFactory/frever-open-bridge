namespace Bridge.Services._7Digital.Models
{
    internal sealed class Release
    {
        public int id { get; set; }
        public string title { get; set; }
        public string version { get; set; }
        public string type { get; set; }
        public Artist artist { get; set; }
        public string slug { get; set; }
        public string image { get; set; }
        public Label label { get; set; }
        public Licensor licensor { get; set; }
        public double popularity { get; set; }
    }
}
