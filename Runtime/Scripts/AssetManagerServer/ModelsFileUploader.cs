using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common.Files;
using Bridge.Results;
using Bridge.Services.AssetService;
using UnityEngine;

namespace Bridge.AssetManagerServer
{
    internal sealed class ModelsFileUploader
    {
        private readonly IAssetService _assetService;

        public ModelsFileUploader(IAssetService assetService)
        {
            _assetService = assetService;
        }
        
        public Task<Result> UploadFiles(ICollection<FileInfo> allFiles, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => UploadFilesAsync(allFiles, cancellationToken), cancellationToken);
        }

        private async Task<Result> UploadFilesAsync(ICollection<FileInfo> allFiles, CancellationToken cancellationToken)
        {
            if (allFiles == null || !allFiles.Any())
            {
                Debug.LogWarning($"Files collection are empty");
                return new SuccessResult();
            }

            var filesToDeploy = new List<FileInfo>();

            var alreadyInUploadingProcess = allFiles.Where(x => x.State == FileState.Uploading).ToArray();

            foreach (var file in allFiles)
            {
                if (file.State != FileState.ModifiedLocally)
                    continue;

                file.TasAsInUploadingProcess();
                filesToDeploy.Add(file);
                file.Extension = file.Extension != FileExtension.Null
                    ? file.Extension
                    : FileExtensionManager.GetFileExtension(file.FilePath);
            }
            
            if (filesToDeploy.Count == 0)
                return new SuccessResult();

            var resp = await _assetService.UploadFilesAsync(filesToDeploy.ToArray(), cancellationToken);
            if (resp.Any(x => !x.IsSuccess))
                return resp.First(x => !x.IsSuccess);

            foreach (var fileInfo in filesToDeploy)
            {
                fileInfo.TagAsDeployed();
            }

            await WaitUploading(alreadyInUploadingProcess);

            return new SuccessResult();
        }


        private async Task WaitUploading(FileInfo[] targets)
        {
            if (targets.Length == 0)
                return;

            while (targets.Any(x=>x.State == FileState.Uploading))
            {
                await Task.Delay(200);
            }
        }
    }
}