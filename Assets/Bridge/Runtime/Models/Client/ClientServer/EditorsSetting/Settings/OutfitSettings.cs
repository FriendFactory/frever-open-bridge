namespace Bridge.Models.ClientServer.EditorsSetting.Settings
{
    public class OutfitSettings: ILevelEditorSetting, IPostRecordEditorSetting
    {
        public bool AllowEditing { get; set; }
        public bool AllowForOwnCharacters { get; set; }
        public bool AllowForFreverStars { get; set; }
        public bool AllowForFriendCharacters { get; set; }
        public bool AllowCreateNew { get; set; }
    }
}