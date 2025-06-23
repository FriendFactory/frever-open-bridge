namespace Bridge.VideoServer
{
    public class AddCommentRequest
    {
        public string Text { get; set; }
        
        public long? ReplyToCommentId { get; set; }
    }
}