namespace Bridge.NotificationServer
{
    public enum NotificationType
    {
         /// <summary>
        /// Received when you get new follower.
        ///
        /// Sent to: user who have new follower
        /// Data Group ID: new follower group
        /// </summary>
        NewFollower,

        /// <summary>
        /// Received when you get new like on your video
        ///
        /// Sent to: video owner
        /// Data Group ID: user who liked video
        /// Data Video ID: video were liked
        /// </summary>
        NewLikeOnVideo,

        /// <summary>
        /// Received when you're tagged on video
        ///
        /// Sent to: user were tagged
        /// Data Group ID: owner of video you're tagged on
        /// Data Video ID: video you're tagged on
        /// </summary>
        YouTaggedOnVideo,

        /// <summary>
        /// Received when your video were remixed
        ///
        /// Sent to: owner of remix source video
        /// Data Group ID: user who made a remix
        /// Data Video ID: remixed video ID (you could get a original video via RemixedFrom property)
        /// </summary>
        YourVideoRemixed,

        /// <summary>
        /// New video is posted by friend (user you mutually follows)
        ///
        /// Send to: all friends
        /// Data Group ID: video owner
        /// Data Video ID: ID of new video
        /// </summary>
        NewFriendVideo,

        /// <summary>
        /// New comment is added on your video.
        ///
        /// Send to: owner of the video
        /// Data group ID: user who commented
        /// Data video ID: ID of video commented
        /// Data ref ID: ID of the comment
        /// </summary>
        NewCommentOnVideo,

        /// <summary>
        /// New comment is added on video you've added comment to before.
        ///
        /// Send to: all user who commented video except video owner (there is separated notification for him)
        /// and comment author.
        ///
        /// DataGroupId: user who commented
        /// DataVideoId: video been added comment to
        /// DataRefId: ID of the comment
        /// DataAssetId: ID of the comment replied
        /// </summary>
        NewCommentOnVideoYouHaveCommented,

        /// <summary>
        /// Video is converted.
        ///
        /// Send to: video owner.
        ///
        /// DataGroupId: user who owns the video
        /// DataVideoId: video has been converted
        /// </summary>
        YourVideoConverted,

        NumberOfRemixesOnVideo,
        Welcome,
        NewAssetsInWardrobe,
        NewScenesInApp,
        FirstFollower,
        NewVideoInCategory,
        NumberOfLikesOnVideo,
        NewTrendVideo,

        /// <summary>
        /// Received when your video was deleted from CMS
        ///
        /// Sent to: video owner
        /// Data Video ID: video were deleted
        /// </summary>
        VideoDeleted,

        /// <summary>
        ///     Received when you're mentioned in comment to video.
        ///     DataGroupId: user who commented
        ///     DataVideoId: video been added comment to
        ///     DataRefId: ID of the comment
        ///     DataRefGroupId: ID of mentioned group
        /// </summary>
        NewMentionInCommentOnVideo,
        
        /// <summary>
        ///     Received when you're mentioned in video description.
        ///     DataGroupId: user who commented
        ///     DataVideoId: video been added comment to
        ///     DataRefGroupId: ID of mentioned group
        /// </summary>
        NewMentionOnVideo,
        
        /// <summary>
        /// Receiving when user status has been changed.
        ///
        /// DataGroupId: user which status were changed.
        /// </summary>
        NewStatusReached,
        
        /// <summary>
        /// Receiving when user level has been changed.
        ///
        /// DataGroupId: user which level were changed.
        /// </summary>
        NewLevelReached,
        
        /// <summary>
        /// Receiving when user accomplish conditions for season quest.
        ///
        /// DataRefId: completed season quest 
        /// </summary>
        SeasonQuestAccomplished,
        
        InvitationAccepted,
        
        /// <summary>
        ///     Receiving when you're (non-character tag) tagged on video
        /// </summary>
        NonCharacterTagOnVideo,
        
        /// <summary>
        /// Receiving when results for style battle that user participated in are ready
        /// </summary>
        BattleResultCompleted,
        
        /// <summary>
        /// Receiving when user get invited to crew
        /// </summary>
        CrewInvitationReceived,
        
        /// <summary>
        /// Receiving when user's request to join crew was approved
        /// </summary>
        CrewJoinRequestAccepted,

        /// <summary>
        /// Receiving when user requests to join your crew
        /// </summary>
        CrewJoinRequestReceived,
        
        /// <summary>
        /// Receiving when your friend has joined your crew
        /// </summary>
        FriendJoinedCrew,
        
        /// <summary>
        ///     Notification send to user after video rating completed
        ///     Sent to: video owner
        ///     DataVideoId: video Id
        /// </summary>
        VideoRatingCompleted,
        
        VideoStyleTransformed
    }
}