using System;
using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Services.AssetService.Caching.CachePathGeneration;

namespace Bridge.Services.AssetService
{
    internal sealed class AssetUrlProvider
    {
        private readonly string _host;
        private readonly Dictionary<Type, AssetSetting> _assetsSettings;

        public AssetUrlProvider(string host, Dictionary<Type, AssetSetting> assetsSettings)
        {
            _host = host;
            _assetsSettings = assetsSettings;
        }

        public string GetUrl(IFilesAttachedEntity model, FileInfo fileInfo)
        {
            return GetUrl(model.GetType(), model.Id, fileInfo);
        }

        public string GetUrl(Type entityType, long entityId, FileInfo fileInfo)
        {
            if (!_assetsSettings.TryGetValue(entityType, out var settings))
            {
                throw new InvalidOperationException($"Failed build url for asset {entityType.Name}");
            }
            
            var url = $"{_host}Cdn/{settings.UnifiedAssetTypeName}/{entityId}/{fileInfo.FileType}";
            
            switch (fileInfo.FileType)
            {
                case FileType.Thumbnail:
                    url += $"/{GetResolutionName(fileInfo.Resolution.Value)}";
                    break;
                case FileType.MainFile:
                    url += $"/{fileInfo.Platform ?? Platform.iOS}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            url += $"/{fileInfo.Version}";

            return url.FixUrlSlashes();
        }

        private string GetResolutionName(Resolution resolution)
        {
            return resolution.ToString().Replace("_", string.Empty);
        }
    }
}