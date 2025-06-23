namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class VfxSettings: ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowEditing { get; set; }

        public long[] Categories { get; set; }
    }
}