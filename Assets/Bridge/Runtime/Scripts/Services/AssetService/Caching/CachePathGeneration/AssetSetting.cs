using System.Linq;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal class AssetSetting
    {
        public readonly FileSettings[] FileSettings;
        public readonly string UnifiedAssetTypeName;
        
        public bool IsPlatformDependent
        {
            get
            {
                var settings = FileSettings.FirstOrDefault(x => x.FileType == FileType.MainFile);
                return settings != null && settings.IsPlatformDependent;
            }
        }

        public AssetSetting(string unifiedAssetTypeName, params FileSettings[] fileSettings)
        {
            UnifiedAssetTypeName = unifiedAssetTypeName;
            FileSettings = fileSettings;
        }
    }
}