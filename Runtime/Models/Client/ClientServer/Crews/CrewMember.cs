using System;
using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewMember
    {
        public GroupShortInfo Group { get; set; }
        public long RoleId { get; set; }
        public bool IsOnline { get; set; }
        public int Trophies { get; set; }
        [ProtoNewField(1)] public bool IsFriend { get; set; }
        [ProtoNewField(2)] public bool IsFollowing { get; set; }
        [ProtoNewField(3)] public bool IsFollower { get; set; }
        [ProtoNewField(4)] public DateTime JoinedCrewTime { get; set; }
        [ProtoNewField(5)] public DateTime LastLoginTime { get; set; }

        public bool IsFriendOrFollower => IsFriend || IsFollower || IsFollowing;
    }
}