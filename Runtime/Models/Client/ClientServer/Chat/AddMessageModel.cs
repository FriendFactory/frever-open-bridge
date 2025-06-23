using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Chat
{
    public class AddMessageModel: IChatMessageModel
    {
        public string Text { get; set; }
        public long? VideoId { get; set; }
        public long? ReplyToMessageId { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}