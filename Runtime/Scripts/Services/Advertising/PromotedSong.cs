using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Services.Advertising
{
    public class PromotedSong: IEntity, IThumbnailOwner 
    {
        public long Id { get; set; }
        public long? SongId { get; set; }
        public long? ExternalSongId { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}