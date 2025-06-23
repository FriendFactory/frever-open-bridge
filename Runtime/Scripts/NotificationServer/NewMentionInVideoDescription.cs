namespace Bridge.NotificationServer
{
    public sealed class NewMentionOnVideoNotification: VideoNotificationBase
    {
        public VideoInfo MentionedVideo { get; set; }

        public GroupInfo MentionedBy { get; set; }
    }
}