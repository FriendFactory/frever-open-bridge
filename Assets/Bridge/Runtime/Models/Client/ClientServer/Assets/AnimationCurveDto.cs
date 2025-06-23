namespace Bridge.Models.ClientServer.Assets
{
    public sealed class AnimationCurveDto
    {
        public Keyframe[] Keys { get; set; }
    }

    public class Keyframe
    {
        public float Time { get; set; }
        public float Value { get; set; }
        public float InTangent { get; set; }
        public float OutTangent { get; set; }
        public int TangentMode { get; set; }
        public int WeightedMode { get; set; }
        public float InWeight { get; set; }
        public float OutWeight { get; set; }
    }
}