using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    /// <summary>
    /// Responsible for files updating
    /// </summary>
    internal interface IEntityFilesService<T> where T: IEntity, IFilesAttachedEntity
    {
        Task<FileUpdateResult> UpdateFilesAsync(long id, List<FileInfo> files, CancellationToken cancellationToken);
    }
}