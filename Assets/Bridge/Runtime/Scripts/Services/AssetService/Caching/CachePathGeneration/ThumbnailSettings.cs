using System;
using System.Linq;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal sealed class ThumbnailSettings : ImageSettings
    {
        private static readonly FileExtension[] AllowedExtensions = { FileExtension.Png, FileExtension.Gif, FileExtension.Jpg, FileExtension.Jpeg };
        
        public ThumbnailSettings(FileExtension extension, Resolution resolution) :
            base(FileType.Thumbnail,new []{extension}, resolution,"Thumbnail")
        {
            if (AllowedExtensions.All(x => x != extension))
            {
                throw new Exception($"Thumbnail can't have {extension.ToString()} extension.");
            }
        }
    }
}