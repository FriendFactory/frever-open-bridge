using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Invitation
{
    public class InviteeReward
    {
        public long InviterGroupId { get; set; }
        public string InviterNickName { get; set; }
        public List<FileInfo> InviterCharacterThumbnail { get; set; }
        public int SoftCurrency { get; set; }
        public string WelcomeMessage { get; set; }
    }
}