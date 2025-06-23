namespace Bridge.NotificationServer
{
    public sealed class SeasonQuestAccomplishedNotification: NotificationBase
    {
        public long SeasonQuestId { get; set; }

        public int LikeCount { get; set; }
    }
}