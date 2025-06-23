using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class PhotoFullInfo: IFilesAttachedEntity
    {
        public long Id { get; set; }

        public int ResolutionWidth { get; set; }

        public int ResolutionHeight { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}