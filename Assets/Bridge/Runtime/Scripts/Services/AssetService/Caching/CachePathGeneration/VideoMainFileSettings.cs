using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal sealed class VideoMainFileSettings: FileSettings
    {
        public VideoMainFileSettings(): base("Video", new []{FileExtension.Mp4, FileExtension.Mov}, false, FileType.MainFile, null)
        {
        }
    }
}