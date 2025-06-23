namespace Bridge.Models.ClientServer.Gamification
{
    public class SeasonLevel
    {
        public long Id { get; set; }
        public int Level { get; set; }
        public int XpRequired { get; set; }
        public UserLevelType LevelType { get; set; }
        public SeasonReward[] Rewards { get; set; }
    }
    
    public enum UserLevelType
    {
        Public = 1,
        Hidden = 2,
        Disabled = 3
    }
}