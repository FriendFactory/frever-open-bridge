using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.Common;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityWriteServices
{
    internal interface IFilePreUploadService
    {
        Task<Result> PreUploadFiles(IEntity target, bool withChildModels, CancellationToken cancellationToken);
    }
}