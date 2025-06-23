using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal sealed class TextFileSettings : FileSettings
    {
        public TextFileSettings(string name) : base(name, FileExtension.Txt, false, FileType.MainFile,  null)
        {

        }
    }
}