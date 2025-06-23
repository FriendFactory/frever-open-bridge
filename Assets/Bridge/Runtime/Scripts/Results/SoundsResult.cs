using Bridge.Models.ClientServer.Assets;
using Bridge.Services._7Digital.Models.TrackModels;

namespace Bridge.Results
{
    public class SoundsResult
    {
        public SongInfo[] Songs { get; set; }
        public UserSoundFullInfo[] UserSounds { get; set; }
        public ExternalSongShortInfo[] ExternalSongs { get; set; }
    }
}