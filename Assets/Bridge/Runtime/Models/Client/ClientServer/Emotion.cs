using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer
{
    public class Emotion: IFilesAttachedEntity
    {
        public long Id { get; set; }
        public string EmojiCode { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}