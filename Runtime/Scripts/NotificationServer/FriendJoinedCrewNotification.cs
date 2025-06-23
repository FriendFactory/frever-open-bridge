namespace Bridge.NotificationServer
{
    public class FriendJoinedCrewNotification : NotificationBase
    {
        public GroupInfo Friend { get; set; }
        public CrewInfo Crew { get; set; }
    }
}