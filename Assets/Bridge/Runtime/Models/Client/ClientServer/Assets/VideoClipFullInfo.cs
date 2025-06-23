using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class VideoClipFullInfo: IFilesAttachedEntity
    {
        public long? Size { get; set; }

        public int? Duration { get; set; }

        public int? FrameRate { get; set; }

        public int ResolutionWidth { get; set; }

        public int ResolutionHeight { get; set; }
        public long Id { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}