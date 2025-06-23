using System.Collections.Generic;
using Bridge.Models.VideoServer;

namespace Bridge.VideoServer
{
    public sealed class DeployLevelVideoReq
    {
        public string LocalPath { get; set; }
        public long LevelId { get; set; }
        public int DurationSec { get; set; }
        public bool IsPublic { get; set; }
        public VideoAccess? Access { get; set; }
        public string VideoDescription { get; set; }
        public VideoOrientation VideoOrientation { get; set; }
        public bool GenerateTemplate { get; set; }
        public string GenerateTemplateWithName { get; set; }
        public long PublishTypeId { get; set; }
        public long[] TaggedFriendIds { get; set; }
        public Dictionary<string, string> Links { get; set; }
        public bool AllowRemix { get; set; }
        public bool AllowComment { get; set; }
        
        public StyleTransformRequest StyleTransformRequest { get; set; }
    }
}