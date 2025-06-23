using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class SongInfo: INamed, IMainFileContainable, IThumbnailOwner, IPlayableMusic, INewTrackable
    {
        public long Id { get; set; }
        public long GenreId { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public ArtistInfo Artist { get; set; }
        public AlbumInfo Album { get; set; }
        public bool IsNew { get; set; }
        [ProtoNewField(1)] public int UsageCount { get; set; }
        [ProtoNewField(2)] public bool IsFavorite { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}