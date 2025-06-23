namespace Bridge.NotificationServer
{
    public sealed class YourVideoConversionCompletedNotification: VideoNotificationBase
    {
        public VideoInfo ConvertedVideo { get; set; }

        public GroupInfo Owner { get; set; }
    }
}