namespace Bridge.Models.ClientServer.Assets
{
    public class BodyAnimationAndVfxDto
    {
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public BodyAnimationInfo BodyAnimation { get; set; }
        public VfxInfo Vfx { get; set; }
    }
}