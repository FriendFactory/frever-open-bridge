using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CreateUserSoundModel
    {
        public int Duration { get; set; }

        public long? Size { get; set; }

        public List<FileInfo> Files { get; set; }

        public string CopyrightCheckResults { get; set; }
    }
}