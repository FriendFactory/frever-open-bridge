using System;
using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.VideoServer
{
    public class VideoShareInfo
    {
        public long VideoId { get; set; }

        public string GroupNickName { get; set; }

        public DateTime CreatedTime { get; set; }

        public string VideoFileUrl { get; set; }

        public long OwnerMainCharacterId { get; set; }

        public List<FileInfo> OwnerMainCharacterThumbnail { get; set; }

        public long Views { get; set; }
    }
}