namespace Bridge.VideoServer
{
    public sealed class VideoCommentAuthorsInfo
    {
        public readonly long[] GroupIds;

        public VideoCommentAuthorsInfo(long[] groupIds)
        {
            GroupIds = groupIds;
        }
    }
}