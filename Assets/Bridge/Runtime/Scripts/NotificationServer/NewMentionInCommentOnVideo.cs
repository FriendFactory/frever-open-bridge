namespace Bridge.NotificationServer
{
    public sealed class NewMentionInCommentOnVideo : VideoNotificationBase
    {
        public VideoInfo CommentedVideo { get; set; }

        public GroupInfo CommentedBy { get; set; }
        
        public CommentInfo Comment { get; set; }
    }
}