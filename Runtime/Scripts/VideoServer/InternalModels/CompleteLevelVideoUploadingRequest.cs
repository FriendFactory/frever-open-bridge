using System;
using System.Collections.Generic;
using Bridge.Models.VideoServer;

namespace Bridge.VideoServer.InternalModels
{
    internal class CompleteLevelVideoUploadingRequest
    {
        public long LevelId { get; set; }

        public long Size { get; set; }

        public int DurationSec { get; set; }
        
        public bool IsPublic { get; set; }

        [Obsolete]
        public bool IsRemixable { get; set; } = true; //todo: drop when we have deployed new field PublishTypeId on Prod 
        
        public string Description { get; set; }
        
        public VideoOrientation VideoOrientation { get; set; }
        
        public VideoAccess? Access { get; set; }
        
        public long[] TaggedFriendIds { get; set; }
        
        public Dictionary<string, string> Links { get; set; }
        
        public bool CreateTemplate { get; set; }
        
        public string CreateTemplateWithName { get; set; }
        
        public long PublishTypeId { get; set; }
        
        [Obsolete]
        public bool IsVideoMessage { get; set; }//todo: drop when we have deployed new field PublishTypeId on Prod 
        public bool AllowRemix { get; set; }
        public bool AllowComment { get; set; }
        
        public StyleTransformRequest StyleTransformRequest { get; set; }
    }
}