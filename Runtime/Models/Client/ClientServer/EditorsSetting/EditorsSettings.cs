namespace Bridge.Models.ClientServer.EditorsSetting
{
    public class EditorsSettings
    {
        public LevelEditorSettings LevelEditorSettings { get; set; }

        public PostRecordEditorSettings PostRecordEditorSettings { get; set; }

        public CharacterEditorSettings CharacterEditorSettings { get; set; }
    }

    public interface IEditorSettings<TEditorSetting> where TEditorSetting: IEditorSetting
    {
        TEditorSetting[] Settings { get; }
    }

    public interface IEditorSetting
    {
    }

    public interface ILevelEditorSetting : IEditorSetting
    {
    }

    public interface IPostRecordEditorSetting : IEditorSetting
    {
    }

    public interface ICharacterEditorSetting : IEditorSetting
    {
    }
}