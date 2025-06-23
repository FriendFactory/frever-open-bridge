using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal sealed class LocalFilesPathProvider
    {
        private Dictionary<Type, AssetSetting> AssetsSettings { get; }
        private readonly FFEnvironment _ffEnvironment;
        private string[] _unifiedAssetTypeNames;

        private string[] UnifiedAssetNames
        {
            get
            {
                return _unifiedAssetTypeNames ?? (_unifiedAssetTypeNames =
                    AssetsSettings.Select(x => x.Value.UnifiedAssetTypeName).Distinct().ToArray());
            }
        }

        public LocalFilesPathProvider(FFEnvironment ffEnvironment)
        {
            _ffEnvironment = ffEnvironment;
            AssetsSettings = Config.Settings;
        }

        public string GetPath(Type assetType, long assetId, FileInfo fileInfo)
        {
            AssetsSettings.TryGetValue(assetType, out var settings);

            if (settings == null)
            {
                throw new Exception($"{assetType.Name} is not supported in cache");
            }

            var fileSettings = FindFileSettings(settings, fileInfo);
            if (fileSettings == null)
                throw new Exception($"Type {assetType.Name} does not have {fileInfo.FileType} {fileInfo.Resolution}");

            var fileName = fileSettings.Name;
            if (fileSettings.AddTagsToFileName && fileInfo.Tags is { Length: > 0 })
            {
                fileName += $"_{fileInfo.Tags.First()}";
            }

            var platform = GetPlatform(fileInfo, settings);
            var platformPrefix = string.Empty;

            if (platform != Platform.Multiplatform)
            {
                platformPrefix = $"{platform}/";
            }

            var fileExtension = fileInfo.Extension.ToExtensionString();

            var path = $"{_ffEnvironment.ToString()}/{settings.UnifiedAssetTypeName}/{assetId}/{platformPrefix}{fileName}{fileExtension}";
            return path;
        }

        public bool IsAssetStorageFolder(string folder)
        {
            return UnifiedAssetNames.Contains(folder);
        }

        private FileSettings FindFileSettings(AssetSetting assetSetting, FileInfo fileInfo)
        {
            return assetSetting.FileSettings.FirstOrDefault(x =>
            {
                var sameResolution = x.Resolution.Compare(fileInfo.Resolution);
                return x.FileType == fileInfo.FileType && sameResolution;
            });
        }

        public string GetPath<T>(T model, FileInfo fileInfo) where T : IFilesAttachedEntity
        {
            var targetType = model.GetType();
            return GetPath(targetType, model.Id, fileInfo);
        }

        private Platform GetPlatform(FileInfo fileInfo, AssetSetting setting)
        {
            if (fileInfo.FileType != FileType.MainFile)
                return Platform.Multiplatform;

            if (!setting.IsPlatformDependent) return Platform.Multiplatform;

            return fileInfo.Platform ?? Platform.iOS;//todo: return fileInfo.Platform when all file info is fixed on backend
        }
    }
}