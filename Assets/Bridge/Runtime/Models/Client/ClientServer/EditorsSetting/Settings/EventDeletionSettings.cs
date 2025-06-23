namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class EventDeletionSettings: ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowDeleting { get; set; }
    }
}