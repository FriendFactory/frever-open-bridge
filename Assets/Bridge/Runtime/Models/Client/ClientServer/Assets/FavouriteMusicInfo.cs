using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class FavouriteMusicInfo: IPlayableMusic
    {
        public long Id { get; set; }
        public SoundType Type { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public int Duration { get; set; }
        [ProtoNewField(1)] public GroupShortInfo Owner { get; set; }
        [ProtoNewField(2)] public int UsageCount { get; set; }
        
        public List<FileInfo> Files { get; set; }
    }
    
    public enum SoundType
    {
        Song,
        ExternalSong,
        UserSound
    }
}