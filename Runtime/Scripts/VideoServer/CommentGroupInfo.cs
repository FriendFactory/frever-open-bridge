namespace Bridge.VideoServer
{
    public sealed class CommentGroupInfo
    {
        /// <summary>
        ///     Gets or sets ID of comment replied
        /// </summary>
        public long CommentId { get; set; }

        /// <summary>
        ///     Gets or sets group ID of user had been written the comment replied
        /// </summary>
        public long GroupId { get; set; }
        
        public int CreatorScoreBadge { get; set; }

        /// <summary>
        ///     Gets or sets group nickname of user had been written the comment replied
        /// </summary>
        public string GroupNickname { get; set; }
    }
}