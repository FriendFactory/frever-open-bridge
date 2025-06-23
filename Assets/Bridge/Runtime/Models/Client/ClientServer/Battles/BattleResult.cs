using Bridge.Models.VideoServer;

namespace Bridge.Models.ClientServer.Battles
{
    public sealed class BattleResult
    {
        public Video Video { get; set; }

        public float Score { get; set; }

        public int SoftCurrency { get; set; }
        
        public GroupInfo Group { get; set; }
    }
}