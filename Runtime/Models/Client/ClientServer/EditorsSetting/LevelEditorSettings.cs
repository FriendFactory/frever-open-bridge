using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.EditorsSetting.Settings;

namespace Bridge.Models.ClientServer.EditorsSetting
{
    public sealed class LevelEditorSettings: IEditorSettings<ILevelEditorSetting>
    {
        public BodyAnimationSettings BodyAnimationSettings { get; set; }

        public CharacterSettings CharacterSettings { get; set; }

        public EventDeletionSettings EventDeletionSettings { get; set; }

        public MusicSettings MusicSettings { get; set; }

        public NonLevelVideoUploadSettings NonLevelVideoUploadSettings { get; set; }

        public PreviewLastEventSettings PreviewLastEventSettings { get; set; }

        public SetLocationSettings SetLocationSettings { get; set; }

        public TemplateSettings TemplateSettings { get; set; }

        public VfxSettings VfxSettings { get; set; }

        public VoiceFilterSettings VoiceFilterSettings { get; set; }

        public OutfitSettings OutfitSettings { get; set; }

        public CameraAnimationSettings CameraAnimationSettings { get; set; }

        public CameraFilterSettings CameraFilterSettings { get; set; }

        public FaceTrackingSettings FaceTrackingSettings { get; set; }
        
        [ProtoNewField(1)] public VideoMessageSettings VideoMessageSettings { get; set; }

        public ILevelEditorSetting[] Settings => this.AllSettings();
    }
    
    public class FaceTrackingSettings: ILevelEditorSetting
    {
        public bool AllowSwitching { get; set; }
    }
    
    public class VideoMessageSettings: ILevelEditorSetting
    {
        public bool AllowSwitch { get; set; }
    }
}