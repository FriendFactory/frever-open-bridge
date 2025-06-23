namespace Bridge.Models.ClientServer.Gamification
{
    public class SeasonQuest
    {
        public static readonly string KnownTypeSeasonLikes = "SeasonLikes";

        public long Id { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public int Value { get; set; }

        public int Xp { get; set; }

        public SeasonReward[] Rewards { get; set; }
    }
}