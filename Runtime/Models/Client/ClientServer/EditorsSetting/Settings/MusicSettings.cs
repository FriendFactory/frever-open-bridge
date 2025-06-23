namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class MusicSettings: ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowEditing { get; set; }

        public SongSettings SongSettings { get; set; }

        public ExternalSongSettings ExternalSongSettings { get; set; }

        public UserSoundSettings UserSoundSettings { get; set; }
    }
}