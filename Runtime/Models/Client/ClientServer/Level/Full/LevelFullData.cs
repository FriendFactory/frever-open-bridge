using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class LevelFullData
    {
        public LevelFullInfo Level { get; set; }

        public List<SetLocationFullInfo> SetLocations { get; set; }

        public List<CharacterFullInfo> Characters { get; set; }

        public List<BodyAnimationFullInfo> BodyAnimations { get; set; }

        public List<CameraFilterInfo> CameraFilters { get; set; }

        public List<VoiceFilterFullInfo> VoiceFilters { get; set; }

        public List<OutfitFullInfo> Outfits { get; set; }

        public List<SongFullInfo> Songs { get; set; }
        
        public List<ExternalSongInfo> ExternalSongs { get; set; }

        public List<VfxFullInfo> Vfx { get; set; }
        
        [ProtoNewField(1)] public List<SetLocationBackground> SetLocationBackgrounds { get; set; }
    }
}