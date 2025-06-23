namespace Bridge.Models.ClientServer.UserActivity
{
    public class UserActivitySettings
    {
        public OriginalVideoCreatedConfiguration OriginalVideoCreated { get; set; } = new OriginalVideoCreatedConfiguration();
        public TemplateVideoCreatedConfiguration TemplateVideoCreated { get; set; } = new TemplateVideoCreatedConfiguration();
        public TaskCompletionConfiguration TaskCompletion { get; set; } = new TaskCompletionConfiguration();
        public VideoLikeConfiguration VideoLike { get; set; } = new VideoLikeConfiguration();
        public VideoWatchConfiguration VideoWatch { get; set; } = new VideoWatchConfiguration();
        public LoginConfiguration Login { get; set; } = new LoginConfiguration();
    }

    public abstract class UserActionConfigurationBase
    {
        public abstract UserActionType ActionType { get; }
    }

    public class OriginalVideoCreatedConfiguration : UserActionConfigurationBase
    {
        public override UserActionType ActionType => UserActionType.OriginalVideoCreated;
    }


    public class TemplateVideoCreatedConfiguration : UserActionConfigurationBase
    {
        public override UserActionType ActionType => UserActionType.TemplateVideoCreated;
    }

    public class TaskCompletionConfiguration : UserActionConfigurationBase
    {
        public override UserActionType ActionType => UserActionType.CompleteTask;
    }

    public class VideoLikeConfiguration : UserActionConfigurationBase
    {
        public override UserActionType ActionType => UserActionType.LikeVideo;
    }

    public class VideoWatchConfiguration : UserActionConfigurationBase
    {
        public override UserActionType ActionType => UserActionType.WatchVideo;
    }

    public class LoginConfiguration : UserActionConfigurationBase
    {
        public override UserActionType ActionType => UserActionType.Login;
    }
}