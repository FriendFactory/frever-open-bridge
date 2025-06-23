namespace Bridge.NotificationServer
{
    public class YourVideoRemixedNotification : VideoNotificationBase
    {
        public VideoInfo Remix { get; set; }

        public VideoInfo RemixedFromVideo { get; set; }

        public GroupInfo RemixedBy { get; set; }
    }
}