namespace Bridge.Models.ClientServer.Assets
{
    public class TrendingUserSound 
    {
        public int UsageCount { get; set; }
        public GroupShortInfo Owner { get; set; }
        public UserSoundFullInfo UserSound { get; set; }
    }
}