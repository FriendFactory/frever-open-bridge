using System;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal sealed class EventCleaner: GenericCleaner<Event>
    {
        protected override Type[] AllowedTypesToCreate { get; } =
        {
            typeof(CameraAnimation),
            typeof(CameraController),
            typeof(CharacterController),
            typeof(CharacterControllerBodyAnimation),
            typeof(CharacterControllerFaceVoice),
            typeof(CharacterControllerUmaRecipe),
            typeof(FaceAnimation),
            typeof(MusicController),
            typeof(SetLocationController),
            typeof(VfxController),
            typeof(VoiceTrack),
            typeof(VideoClip),
            typeof(Photo),
            typeof(CameraFilterController),
            typeof(Caption)
        };
    }
}