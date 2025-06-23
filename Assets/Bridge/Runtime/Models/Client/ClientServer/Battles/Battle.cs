using Bridge.Models.VideoServer;

namespace Bridge.Models.ClientServer.Battles
{
    public class Battle
    {
        public long Id { get; set; }
        public BattleVideo[] Videos { get; set; }
    }

    public class BattleVideo
    {
        public Video Video { get; set; }

        public int WinScore { get; set; }

        public int LossScore { get; set; }
        
        public string FileUrl { get; set; }
    }
}
