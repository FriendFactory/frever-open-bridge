namespace Bridge.NotificationServer
{
    public class CrewJoinRequestAcceptedNotification : NotificationBase
    {
        public CrewInfo Crew { get; set; }
    }
}