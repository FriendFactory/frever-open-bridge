using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewMemberRequest
    {
        public GroupShortInfo Group { get; set; }
        public string UserPitch { get; set; }
        [ProtoNewField(1)] public long Id { get; set; }
    }
}