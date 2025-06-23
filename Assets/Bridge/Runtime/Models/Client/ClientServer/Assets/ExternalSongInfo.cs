namespace Bridge.Models.ClientServer.Assets
{
    public class ExternalSongInfo
    {
        public long ExternalTrackId { get; set; }

        public string SongName { get; set; }

        public string ArtistName { get; set; }

        public int Duration { get; set; } = 30;
    }
}