namespace Bridge.NotificationServer
{
    public class CommentInfo
    {
        public long Id { get; set; }

        public string Key { get; set; }

        public GroupInfo CommentedBy { get; set; }

        public CommentInfo ReplyTo { get; set; }
    }
}