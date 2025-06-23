using System;
using System.Collections.Generic;
using Bridge.Models.VideoServer;

namespace Bridge.VideoServer
{
    public sealed class DeployNonLevelVideoReq
    {
        public readonly string LocalPath;
        public readonly int DurationSec;
        public readonly bool IsPublic;
        public readonly VideoAccess? Access;
        public readonly string Description;
        public readonly long[] TaggedFriendIds;
        public readonly Dictionary<string, string> Links;
        public readonly long PublishTypeId;
        public bool AllowRemix => false;
        public bool AllowComment { get; set; }

        public DeployNonLevelVideoReq(string localPath, int durationSec, bool isPublic, long publishTypeId, string description = null, long[] taggedFriendIds = null, Dictionary<string, string> links = null)
        {
            LocalPath = localPath ?? throw new ArgumentNullException(nameof(localPath));
            DurationSec = durationSec;
            IsPublic = isPublic;
            PublishTypeId = publishTypeId;
            TaggedFriendIds = taggedFriendIds;
            Description = description;
            Links = links;
        }
        
        public DeployNonLevelVideoReq(string localPath, int durationSec, VideoAccess access, long publishTypeId, string description = null, long[] taggedFriendIds = null, Dictionary<string, string> links = null)
        {
            LocalPath = localPath ?? throw new ArgumentNullException(nameof(localPath));
            DurationSec = durationSec;
            Access = access;
            PublishTypeId = publishTypeId;
            TaggedFriendIds = taggedFriendIds;
            Description = description;
            IsPublic = access == VideoAccess.Public;
            Links = links;
        }
    }
}