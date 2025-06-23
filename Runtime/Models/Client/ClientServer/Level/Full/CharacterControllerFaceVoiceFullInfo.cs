using Bridge.Models.ClientServer.Assets;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class CharacterControllerFaceVoiceFullInfo
    {
        public long Id { get; set; }

        public long? VoiceFilterId { get; set; }

        public int VoiceSoundVolume { get; set; }

        public VoiceTrackFullInfo VoiceTrack { get; set; }

        public FaceAnimationFullInfo FaceAnimation { get; set; }
    }
}