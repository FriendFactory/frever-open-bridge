namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class VoiceFilterSettings : ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowEditing { get; set; }

        public bool AllowDisablingVoiceFilter { get; set; }
        
        public long[] Ids { get; set; }
    }
}