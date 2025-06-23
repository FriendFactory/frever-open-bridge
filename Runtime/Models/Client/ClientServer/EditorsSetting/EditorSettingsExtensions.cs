using System.Linq;

namespace Bridge.Models.ClientServer.EditorsSetting
{
    /// <summary>
    /// Can't use iterator pattern and base class for Editor Settings because of limitations with protobuf deserialization
    /// Must use extensions instead
    /// </summary>
    internal static class EditorSettingsExtensions
    {
        public static ICharacterEditorSetting[] AllSettings(this CharacterEditorSettings characterEditorSettings) 
        {
            return AllSettingsGeneric<ICharacterEditorSetting>(characterEditorSettings);
        }
    
        public static ILevelEditorSetting[] AllSettings(this LevelEditorSettings levelEditorSettings) 
        {
            return AllSettingsGeneric<ILevelEditorSetting>(levelEditorSettings);
        }
    
        public static IPostRecordEditorSetting[] AllSettings(this PostRecordEditorSettings postRecordEditorSettings)
        {
            return AllSettingsGeneric<IPostRecordEditorSetting>(postRecordEditorSettings);
        }

        private static T[] AllSettingsGeneric<T>(object target)
        {
            var fields = target.GetType().GetProperties()
                .Where(x => typeof(T).IsAssignableFrom(x.PropertyType));
            return fields.Select(x => x.GetValue(target)).Where(x => x != null).Cast<T>()
                .ToArray();
        }
    }
}