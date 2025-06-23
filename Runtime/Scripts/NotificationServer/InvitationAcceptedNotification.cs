namespace Bridge.NotificationServer
{
    public class InvitationAcceptedNotification : NotificationBase
    {
        public GroupInfo AcceptedBy { get; set; }
        public RewardInfo Reward { get; set; }
    }

    public class RewardInfo
    {
        public bool IsClaimed { get; set; }
        public int SoftCurrency { get; set; }
    }
}