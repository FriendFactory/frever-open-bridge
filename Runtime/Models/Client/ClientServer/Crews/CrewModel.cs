using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewModel : IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ChatId { get; set; }
        public int MembersCount { get; set; }
        public int TotalMembersCount { get; set; }
        public CrewMember[] Members { get; set; }
        public CrewCompetition Competition { get; set; }
        public List<FileInfo> Files { get; set; }
        public string MessageOfDay { get; set; }
        public bool IsPublic { get; set; }
        [ProtoNewField(1)] public bool IsInvited { get; set; }
        [ProtoNewField(2)] public bool IsJoinRequested { get; set; }
        [ProtoNewField(3)] public long? LanguageId { get; set; }
    }
}