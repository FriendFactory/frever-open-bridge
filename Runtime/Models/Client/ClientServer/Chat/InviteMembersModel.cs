using System.Collections.Generic;

namespace Bridge.Models.ClientServer.Chat
{
    public class InviteMembersModel
    {
        public long ChatId { get; set; }
        public List<long> GroupIds { get; set; } = new List<long>();
    }
}