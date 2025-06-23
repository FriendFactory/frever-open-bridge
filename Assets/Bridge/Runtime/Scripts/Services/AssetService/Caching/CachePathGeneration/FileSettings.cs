using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal class FileSettings
    {
        public readonly string Name;
        public readonly FileType FileType;
        public readonly Resolution? Resolution;
        public readonly FileExtension[] Extensions;
        public readonly bool IsPlatformDependent;
        public readonly bool AddTagsToFileName;

        protected FileSettings(string name, FileExtension extension, bool isPlatformDependent, FileType fileType, Resolution? resolution, bool addTagsToFileName = false) : this(name,new[] { extension }, isPlatformDependent, fileType, resolution, addTagsToFileName)
        {
        }

        protected FileSettings(string name, FileExtension[] extensions, bool isPlatformDependent, FileType fileType, Resolution? resolution, bool addTagsToFileName = false)
        {
            Name = name;
            Extensions = extensions;
            IsPlatformDependent = isPlatformDependent;
            FileType = fileType;
            Resolution = resolution;
            AddTagsToFileName = addTagsToFileName;
        }
    }
}