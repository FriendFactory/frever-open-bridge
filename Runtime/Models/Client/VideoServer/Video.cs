using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.VideoServer.Models;

namespace Bridge.Models.VideoServer
{
    public class Video: IEntity
    {
        public long Size { get; set; }

        public int Duration { get; set; }

        public DateTime CreatedTime { get; set; }

        public VideoKPI KPI { get; set; }

        public bool LikedByCurrentUser { get; set; }

        public int CharactersCount { get; set; }

        public long? RemixedFromVideoId { get; set; }

        public long? RemixedFromLevelId { get; set; }

        public int GroupCreatorScoreBadge { get; set; }

        public GroupInfo Owner { get; set; }

        public GroupInfo OriginalCreator { get; set; }

        public long TopListPosition { get; set; }

        public bool IsRemixable { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPinned { get; set; }

        public VideoAccess Access { get; set; }

        public double QualityInitial { get; set; }

        public long[] TemplateIds { get; set; }

        public long? GeneratedTemplateId { get; set; }

        public VideoTemplateInfo MainTemplate { get; set; }

        public long TaskId { get; set; }

        public string TaskName { get; set; }

        public bool IsVotingTask { get; set; }

        public VotingResult BattleResult { get; set; }

        public List<HashtagInfo> Hashtags { get; set; }

        public List<TaggedGroup> Mentions { get; set; }

        public TaggedGroup[] TaggedGroups { get; set; }

        public TaggedGroup[] NonCharacterTaggedGroups { get; set; }
        
        public string SongName { get; set; }

        public string ArtistName { get; set; }

        public long? ExternalSongId { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }

        public string ThumbnailUrl { get; set; }

        public string RedirectUrl { get; set; }

        public Dictionary<string, string> SignedCookies { get; set; }

        public Dictionary<string, string> Links { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public long Id { get; set; }

        public long? LevelId { get; set; }

        public long GroupId { get; set; }

        public string Version { get; set; }
        
        [ProtoNewField(1)] public SongInfo[] Songs { get; set; }
        
        [ProtoNewField(2)] public UserSoundInfo[] UserSounds { get; set; }
        
        [ProtoNewField(3)] public long PublishTypeId { get; set; }
        [ProtoNewField(4)] public TemplateFromVideo TemplateFromVideo { get; set; }
        [ProtoNewField(5)] public long? LevelTypeId { get; set; }
        [ProtoNewField(6)] public bool AllowRemix { get; set; }
        [ProtoNewField(7)] public bool AllowComment { get; set; }
        [ProtoNewField(8)] public bool IsFriend { get; set; }
        [ProtoNewField(9)] public bool IsFollower { get; set; }
        [ProtoNewField(10)] public bool IsFollowed { get; set; }
        [ProtoNewField(11)] public bool IsFollowRecommended { get; set; }
        [ProtoNewField(12)] public RatingResult RatingResult { get; set; }
    }
    
    public class TemplateFromVideo
    {
        public bool AllowFeature { get; set; }
        public RestrictionReason? RestrictionReason { get; set; }
    }
    
    public enum RestrictionReason
    {
        VideoHasTemplate = 0,
        VideoIsRemix = 1,
        VideoWithoutLevel = 2,
        VideoMessageLevelType = 3
    }

    public class VotingResult
    {
        public int Place { get; set; }
        public float Score  { get; set; }
    }
    
    public class VideoTemplateInfo: IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? CreatorGroupId { get; set; }
        public string CreatorNickname { get; set; }
        public long? CreatorCharacterId { get; set; }
        public List<FileInfo> CreatorCharacterFiles { get; set; }
        public long? GeneratedFromVideoId { get; set; }
    }
    
    public class GroupInfo
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public long? MainCharacterId { get; set; }
        public List<FileInfo> MainCharacterFiles { get; set; }
    }
    
    public class SongInfo
    {
        public long Id { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public bool IsExternal { get; set; }

        public string Isrc { get; set; }
    }
    
    public class UserSoundInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long EventId { get; set; }
    }
    
    public class RatingResult
    {
        public int Rating { get; set; }
        public int SoftCurrency { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRewardAvailable { get; set; }
    }
}