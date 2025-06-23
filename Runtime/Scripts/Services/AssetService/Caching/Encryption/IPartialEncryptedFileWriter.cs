using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal interface IPartialEncryptedFileWriter : IEncryptedFileWriter
    {
        Task EncryptFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}