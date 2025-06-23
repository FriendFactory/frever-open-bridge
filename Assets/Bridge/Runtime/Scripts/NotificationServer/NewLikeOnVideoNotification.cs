namespace Bridge.NotificationServer
{
    public class NewLikeOnVideoNotification : VideoNotificationBase
    {
        public VideoInfo LikedVideo { get; set; }

        public GroupInfo LikedBy { get; set; }
    }
}