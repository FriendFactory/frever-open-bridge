namespace Bridge.NotificationServer
{
    public class NewCommentOnVideoYouHaveCommentedNotification : VideoNotificationBase
    {
        public VideoInfo CommentedVideo { get; set; }

        public GroupInfo CommentedBy { get; set; }
        
        public CommentInfo Comment { get; set; }
    }
    
    
}