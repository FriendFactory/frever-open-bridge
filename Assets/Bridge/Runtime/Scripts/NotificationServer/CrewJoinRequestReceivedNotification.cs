using Bridge.ExternalPackages.Protobuf;

namespace Bridge.NotificationServer
{
    public class CrewJoinRequestReceivedNotification: NotificationBase
    {
        public GroupInfo RequestedBy { get; set; }
        public CrewInfo Crew { get; set; }
        [ProtoNewField(1)] public long? RequestId { get; set; }
    }
}