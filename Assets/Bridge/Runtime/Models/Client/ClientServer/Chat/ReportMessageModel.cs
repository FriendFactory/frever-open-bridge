#pragma warning disable CS8618

namespace Bridge.Models.ClientServer.Chat
{
    public class ReportMessageModel
    {
        public long ChatMessageId { get; set; }
        public long ReasonId { get; set; }
        public string ReportText { get; set; }
    }
}