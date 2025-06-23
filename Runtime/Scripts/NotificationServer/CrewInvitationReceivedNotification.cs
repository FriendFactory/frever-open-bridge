namespace Bridge.NotificationServer
{
    public class CrewInvitationReceivedNotification: NotificationBase
    {
        public GroupInfo InvitedBy { get; set; }
        public CrewInfo Crew { get; set; }
    }
}