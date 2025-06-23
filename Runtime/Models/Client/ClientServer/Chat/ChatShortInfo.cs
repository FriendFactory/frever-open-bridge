using System;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Chat
{
    public class ChatShortInfo: IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int NewMessagesCount { get; set; }
        public string LastMessageText { get; set; }
        public GroupShortInfo[] Members { get; set; }
        [ProtoNewField(1)] public DateTime? LastMessageTime { get; set; }
        [ProtoNewField(2)] public ChatType Type { get; set; }
        [ProtoNewField(3)] public long? LastReadMessageId { get; set; }
        [ProtoNewField(4)] public DateTime? MutedUntilTime { get; set; }
    }
}