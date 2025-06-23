using System.Collections.Generic;

namespace Bridge.Models.ClientServer.Chat
{
    public class SaveChatModel
    {
        public string Title { get; set; }
        public List<long> GroupIds { get; set; } = new List<long>();
    }
}