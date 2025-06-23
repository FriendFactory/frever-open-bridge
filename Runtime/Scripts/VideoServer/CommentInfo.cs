using System;
using System.Collections.Generic;
using Bridge.Models.Common;

namespace Bridge.VideoServer
{
    public sealed class CommentInfo: IEntity
    {
        public long Id { get; set; }

        public long VideoId { get; set; }

        public long GroupId { get; set; }

        public DateTime Time { get; set; }

        public string Text { get; set; }

        public string GroupNickname { get; set; }

        public int GroupCreatorScoreBadge { get; set; }

        public List<Mention> Mentions { get; set; }

        /// <summary>
        ///     Use this field to paginate comments
        /// </summary>
        public string Key { get; set; }

        public int ReplyCount { get; set; }

        public CommentGroupInfo ReplyToComment { get; set; }

        public long LikeCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }

        public bool IsPinned { get; set; }

        public bool IsRoot => ReplyToComment == null;
    }
    
    public sealed class Mention
    {
        public long GroupId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }
    }
}