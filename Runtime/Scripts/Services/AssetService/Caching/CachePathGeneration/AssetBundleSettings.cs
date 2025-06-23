using Bridge.Constants;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal sealed class AssetBundleSettings : FileSettings
    {
        public AssetBundleSettings() : base(FileNameConstants.ASSET_BUNDLE_FILE_NAME,new[] { FileExtension.Empty }, true, FileType.MainFile, null)
        {
        }
    }
}