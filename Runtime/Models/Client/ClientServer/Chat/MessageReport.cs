using System;

namespace Bridge.Models.ClientServer.Chat
{
    public class MessageReport
    {
        public long Id { get; set; }
        public long ChatMessageId { get; set; }
        public long GroupId { get; set; }
        public long ReasonId { get; set; }
        public string ReportText { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ClosedTime { get; set; }
        public bool HideMessage { get; set; }
    }
}