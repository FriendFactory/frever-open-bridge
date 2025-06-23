using System;

namespace Bridge.Models.ClientServer.Invitation
{
    public class InvitationCode 
    {
        public string Code { get; set; }
        public Guid InvitationGuid { get; set; }
        public int SoftCurrency { get; set; }
        public string WelcomeMessage { get; set; }
        public InviteGroup[] InviteGroups { get; set; }
    }
}