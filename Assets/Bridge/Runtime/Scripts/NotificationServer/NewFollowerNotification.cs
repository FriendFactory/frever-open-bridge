namespace Bridge.NotificationServer
{
    public class NewFollowerNotification : NotificationBase
    {
        public GroupInfo FollowedBy { get; set; }
    }
}