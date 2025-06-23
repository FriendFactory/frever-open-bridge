using System.Collections.Generic;
using Bridge.Models.VideoServer;

namespace Bridge.VideoServer
{
    internal sealed class CompleteNonLevelVideoUploadingRequest
    {
        public string Description { get; set; }
        
        public int DurationSec { get; set; }

        public long Size { get; set; }

        public bool IsPublic { get; set; }
        
        public VideoAccess? Access { get; set; }

        public bool IsVideoMessage { get; set; }

        public long PublishTypeId { get; set; }

        public long[] TaggedFriendIds { get; set; }
        
        public Dictionary<string, string> Links { get; set; }
        public bool AllowRemix { get; set; }
        public bool AllowComment { get; set; }
    }
}