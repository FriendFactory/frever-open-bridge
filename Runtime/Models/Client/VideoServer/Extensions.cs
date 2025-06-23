namespace Bridge.Models.VideoServer
{
    public static class Extensions
    {
        public static bool IsPublishedAsMessage(this Video video)
        {
            return video.PublishTypeId == ServerConstants.VideoPublishingType.VIDEO_MESSAGE;
        }
    }
}