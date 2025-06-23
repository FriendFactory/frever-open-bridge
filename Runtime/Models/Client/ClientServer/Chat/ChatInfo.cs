using System;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Chat
{
    public class ChatInfo : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? LastReadMessageId { get; set; }
        public GroupShortInfo[] Members { get; set; }
        [ProtoNewField(1)] public ChatType Type { get; set; }
        [ProtoNewField(2)] public int NewMessagesCount { get; set; }
        [ProtoNewField(3)] public string LastMessageText { get; set; }
        [ProtoNewField(4)] public DateTime? LastMessageTime { get; set; }
        [ProtoNewField(5)] public DateTime? MutedUntilTime { get; set; }
    }

    public enum ChatType
    {
        Private = 1,
        Crew = 2,
        Group = 3
    }
}