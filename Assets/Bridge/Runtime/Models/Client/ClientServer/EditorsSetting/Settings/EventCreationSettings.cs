namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class EventCreationSettings: IPostRecordEditorSetting
    {
        public bool AllowEventCreation { get; set; }
        public long? TemplateId { get; set; }
    }
}