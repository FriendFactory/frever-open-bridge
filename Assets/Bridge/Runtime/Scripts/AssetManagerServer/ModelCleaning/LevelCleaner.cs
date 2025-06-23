using System;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal sealed class LevelCleaner : GenericCleaner<Level>
    {
        protected override Type[] AllowedTypesToCreate { get; } =
        {
            typeof(Event), 
            typeof(CharacterController), 
            typeof(FaceAnimation), 
            typeof(VoiceTrack),
            typeof(VfxController), 
            typeof(MusicController), 
            typeof(CharacterControllerFaceVoice),
            typeof(CameraController),
            typeof(CameraAnimation), 
            typeof(CharacterControllerBodyAnimation), 
            typeof(CharacterControllerUmaRecipe),
            typeof(SetLocationController), 
            typeof(VideoClip), 
            typeof(Photo),
            typeof(CameraFilterController),
            typeof(Caption)
        };
    }
}