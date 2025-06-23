namespace Bridge.Models.ClientServer.Chat
{
    public sealed class SendMessageToGroupsModel
    {
        public long[] ChatIds { get; set; }
        public long[] GroupIds { get; set; }
        public string Text { get; set; }
        public long? VideoId { get; set; }
    }
}