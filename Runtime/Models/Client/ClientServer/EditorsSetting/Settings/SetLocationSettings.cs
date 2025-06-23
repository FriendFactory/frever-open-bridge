namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class SetLocationSettings: ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowPhotoUploading { get; set; }
        public bool AllowVideoUploading { get; set; }
        public bool AllowChangeDayTime { get; set; }
        public bool AllowChangeSetLocation { get; set; }
        public long[] Categories { get; set; }
        public long[] Tags { get; set; }
    }
}