namespace Bridge.NotificationServer
{
    public class NewFriendVideoNotification : VideoNotificationBase
    {
        public VideoInfo NewVideo { get; set; }

        public GroupInfo PostedBy { get; set; }
    }
}