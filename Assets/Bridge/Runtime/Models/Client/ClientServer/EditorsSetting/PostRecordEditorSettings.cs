using Bridge.Models.ClientServer.EditorsSetting.Settings;

namespace Bridge.Models.ClientServer.EditorsSetting
{
    public sealed class PostRecordEditorSettings: IEditorSettings<IPostRecordEditorSetting>
    {
        public BodyAnimationSettings BodyAnimationSettings { get; set; }

        public CameraAnimationSettings CameraAnimationSettings { get; set; }

        public CameraFilterSettings CameraFilterSettings { get; set; }

        public CaptionSettings CaptionSettings { get; set; }

        public CharacterSettings CharacterSettings { get; set; }

        public EventCreationSettings EventCreationSettings { get; set; }

        public EventDeletionSettings EventDeletionSettings { get; set; }

        public MusicSettings MusicSettings { get; set; }

        public OutfitSettings OutfitSettings { get; set; }

        public SetLocationSettings SetLocationSettings { get; set; }

        public VfxSettings VfxSettings { get; set; }

        public VoiceFilterSettings VoiceFilterSettings { get; set; }

        public VolumeSettings VolumeSettings { get; set; }

        public IPostRecordEditorSetting[] Settings => this.AllSettings();
    }
}