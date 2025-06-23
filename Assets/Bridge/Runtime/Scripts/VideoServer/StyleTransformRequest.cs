namespace Bridge.VideoServer
{
    public sealed class StyleTransformRequest
    {
        public long StyleId { get; set; }
        public string PositivePrompt { get; set; }
        public string NegativePrompt { get; set; }
    }
}