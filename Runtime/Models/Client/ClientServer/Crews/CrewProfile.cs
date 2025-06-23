using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewProfile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [ProtoNewField(1)] public long RoleId { get; set; }
        [ProtoNewField(2)] public long ChatId { get; set; }
    }
}