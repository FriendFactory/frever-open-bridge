using System;
using System.Linq;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal class ImageSettings : FileSettings
    {
        private static readonly FileExtension[] AllowedExtensions = { FileExtension.Png, FileExtension.Gif, FileExtension.Jpg, FileExtension.Jpeg };
        
        public ImageSettings(FileType fileType, FileExtension[] extensions, Resolution? resolution, string name, bool addTagsToFileName = false) 
            : base(name+resolution, extensions, false, fileType, resolution, addTagsToFileName)
        {
            foreach (var extension in extensions)
            {
                ValidateExtensions(extension);
            }
        }
        
        private void ValidateExtensions(FileExtension extension)
        {
            if (AllowedExtensions.All(x => x != extension))
            {
                throw new Exception($"Thumbnail can't have {extension.ToString()} extension.");
            }
        }
    }
}