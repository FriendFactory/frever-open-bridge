using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AssetManagerServer.EntityReadServices;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.AsseManager.Extensions.FilesContainable;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    internal sealed class FilePreUploadService: BaseEntityWriteService ,IFilePreUploadService
    {
        public FilePreUploadService(string host, IRequestHelper requestHelper, ResponseReaderProvider responseReaderProvider, ContractResolverProvider resolverProvider, ISerializer serializer, 
            ModelCleanerProvider cleanerProvider, ModelsToCacheSaver fileCache, ModelsFileUploader modelsFileUploader) : 
            base(host, requestHelper, responseReaderProvider, resolverProvider, serializer, cleanerProvider, fileCache, modelsFileUploader)
        {
        }
        
        public async Task<Result> PreUploadFiles(IEntity target, bool withChildModels, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => PreUploadFilesInternal(target, withChildModels, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledResult();
            }
        }
        
        private Task<Result> PreUploadFilesInternal(IEntity target, bool withChildModels, CancellationToken cancellationToken)
        {
            ICollection<FileInfo> fileInfos;
            if (!withChildModels)
            {
                if(!(target is IFilesAttachedEntity targetAsFileContainable))
                    throw new InvalidOperationException($"You can't preupload files for non file containable model with skipping childs." +
                                                        $" Entity {target.GetType().Name} does not inherit {nameof(IFilesAttachedEntity)}. To deploy files for child models, please set second parameter to TRUE");
                
                fileInfos = (targetAsFileContainable).Files;
            }
            else
            {
                var targetModels = target.ExtractAllModelWithFiles();
                fileInfos = targetModels.Where(x => x.Files != null)
                    .SelectMany(x => x.Files).ToArray();
            }
            
            return UploadFiles(fileInfos, cancellationToken);
        }
    }
}