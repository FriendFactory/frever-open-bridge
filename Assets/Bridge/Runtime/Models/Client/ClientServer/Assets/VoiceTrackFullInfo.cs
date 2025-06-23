using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class VoiceTrackFullInfo: IMainFileContainable
    {
        public long Id { get; set; }

        public int Duration { get; set; }
        
        public long? VoiceOwnerGroupId { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}