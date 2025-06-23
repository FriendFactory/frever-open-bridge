namespace Bridge.NotificationServer
{
    public class NewCommentOnVideoNotification : VideoNotificationBase
    {
        public VideoInfo CommentedVideo { get; set; }

        public GroupInfo CommentedBy { get; set; }
        
        public CommentInfo Comment { get; set; }
    }
}