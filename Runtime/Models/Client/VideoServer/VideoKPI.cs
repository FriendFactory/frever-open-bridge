using Bridge.Models.Common;

namespace Bridge.VideoServer.Models
{
    public class VideoKPI
    {
        public long VideoId { get; set; }
        public long Likes { get; set; }
        public long Views { get; set; }
        public long Comments { get; set; }
        public long Shares { get; set; }
        public long Remixes { get; set; }
        public long BattlesWon { get; set; }
        public long BattlesLost { get; set; }
    }
}