namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class CameraAnimationSettings: ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowEditing { get; set; }

        public long[] TemplateIds { get; set; }
        
        public bool AllTemplatesAvailable { get; set; }
    }
}