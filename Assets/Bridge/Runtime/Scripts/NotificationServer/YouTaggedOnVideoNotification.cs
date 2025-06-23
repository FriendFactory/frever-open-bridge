namespace Bridge.NotificationServer
{
    public class YouTaggedOnVideoNotification : VideoNotificationBase
    {
        public VideoInfo TaggedOnVideo { get; set; }

        public GroupInfo TaggedBy { get; set; }
    }
}