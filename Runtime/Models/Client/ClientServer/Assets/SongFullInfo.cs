using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class SongFullInfo: IMainFileContainable, IThumbnailOwner, INamed, IPlayableMusic
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public long[] Tags { get; set; }

        public long GenreId { get; set; }

        public long LabelId { get; set; }

        public long ArtistId { get; set; }

        public string ArtistName { get; set; }

        public long? AlbumId { get; set; }

        public string AlbumName { get; set; }

        public int SortOrder { get; set; }

        public bool ParentalExplicit { get; set; }

        public long? MoodId { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}