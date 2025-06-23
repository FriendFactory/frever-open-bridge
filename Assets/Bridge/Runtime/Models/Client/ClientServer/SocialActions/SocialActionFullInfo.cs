using System;
using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.SocialActions
{
    public class SocialActionFullInfo
    {
        public Guid RecommendationId { get; set; }
        public long ActionId { get; set; }
        public SocialActionType ActionType { get; set; }
        public SocialActionVideoInfoShort TargetVideo { get; set; }
        public SocialActionGroupInfoShort TargetGroup { get; set; }
        public long? TargetTemplateId { get; set; }
        public long? TargetTaskId { get; set; }
        public string Reason { get; set; }
        public SocialActionGroupInfoShort[] ReasonGroups { get; set; } 
    }
    
    public class SocialActionGroupInfoShort
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public long? MainCharacterId { get; set; }
        public List<FileInfo> MainCharacterFiles { get; set; }
    } 
    
    public class SocialActionVideoInfoShort
    {
        public long Id { get; set; }
        public string ThumbnailUrl { get; set; }
    }
    
    public enum SocialActionType
    {
        MutualVideo = 0,
        PopularAccount = 1,
        LikelyFriend = 2,
        LikeVideo = 3,
        FollowBack = 4,
        TrendingTemplate = 5,
        StyleBattle = 6
    }
}