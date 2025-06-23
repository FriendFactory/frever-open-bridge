using Bridge.Models.ClientServer.Assets;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class MusicControllerFullInfo
    {
        public long Id { get; set; }
        public long? SongId { get; set; }
        public int ActivationCue { get; set; }
        public int EndCue { get; set; }
        public int LevelSoundVolume { get; set; }
        public long? ExternalTrackId { get; set; }

        public UserSoundFullInfo UserSound { get; set; }
    }
}