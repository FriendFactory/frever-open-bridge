using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class SetLocationControllerFullInfo
    {
        public long Id { get; set; }

        public long SetLocationId { get; set; }

        public int ActivationCue { get; set; }

        public int EndCue { get; set; }

        public long? TimeOfDay { get; set; }

        public int? VideoActivationCue { get; set; }

        public int? VideoEndCue { get; set; }

        public int VideoSoundVolume { get; set; }

        public long? TimelapseSpeed { get; set; }

        public PhotoFullInfo Photo { get; set; }

        public VideoClipFullInfo VideoClip { get; set; }
        
        [ProtoNewField(1)]
        public PictureInPictureSettings PictureInPictureSettings { get; set; }
        [ProtoNewField(2)] public long? SetLocationBackgroundId { get; set; }
    }
    
    public sealed class PictureInPictureSettings
    {
        public Vector2Dto Position { get; set; }
        public float Scale { get; set; }
        [ProtoNewField(1)] public float Rotation { get; set; }
    }
}