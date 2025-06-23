using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal static class Extensions
    {
        public static string ToExtensionString(this FileExtension ext)
        {
            if (ext == FileExtension.Empty)
                return string.Empty;
            
            return "."+ ext.ToString().ToLower();
        }
    }
}