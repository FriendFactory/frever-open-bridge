namespace Bridge.ClientServer.ImageGeneration
{
    public sealed class PhotoTransformationFileInfo
    {
        public bool Ok { get; set; }
        public string ErrorMessage { get; set; }
        public string MainFileUrl { get; set; }
        public string CoverFileUrl { get; set; }
        public string ThumbnailFileUrl { get; set; }
        public string MaskFileUrl { get; set; }
    }
}