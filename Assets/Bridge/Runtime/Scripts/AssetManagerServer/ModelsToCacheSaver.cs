using System.Collections.Generic;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Services.AssetService.Caching;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Bridge.AssetManagerServer
{
    internal sealed class ModelsToCacheSaver
    {
        private readonly AssetsCache _assetsCache;
        public ModelsToCacheSaver(AssetsCache assetsCache)
        {
            _assetsCache = assetsCache;
        }
        
        public async Task SaveFiles(ICollection<IFilesAttachedEntity> targetFiles)
        {
            foreach (var filesContainable in targetFiles)
            {
                await SaveFiles(filesContainable);
            }
        }
        
        public async Task SaveFiles(IFilesAttachedEntity targetFile)
        { 
            foreach (var file in targetFile.Files)
            {
                await SaveFile(targetFile, file);
            }
        }

        private async Task SaveFile(IFilesAttachedEntity filesContainable, FileInfo fileInfo)
        {
            if (!CanSaveFile(fileInfo))
                return;

            await _assetsCache.SaveToCacheAsync(filesContainable, fileInfo);

            fileInfo.TagAsSyncedWithServer();
            fileInfo.ReleaseAssetRawDataFromRAM();
        }

        private bool CanSaveFile(FileInfo fileInfo)
        {
            return fileInfo != null && (fileInfo.State == FileState.PreUploaded ||
                fileInfo.State == FileState.ShouldBeCopiedFromSource);
        }
    }
}