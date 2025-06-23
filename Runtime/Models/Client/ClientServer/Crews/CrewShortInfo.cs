using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewShortInfo : IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FileInfo> Files { get; set; }
        public int MembersCount { get; set; }
        public int TotalMembersCount { get; set; }
        public int TrophyScore { get; set; }
        public bool IsPublic { get; set; }
        [ProtoNewField(1)] public int FriendsCount { get; set; }
        [ProtoNewField(2)] public int FollowingCount { get; set; }
        [ProtoNewField(3)] public int FollowersCount { get; set; }
        [ProtoNewField(4)] public GroupShortInfo[] Members { get; set; }
        [ProtoNewField(5)] public long? LanguageId {get; set; }
    }
}