using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal interface IPartialEncryptedFileReader : IEncryptedFileReader
    {
        Task DecryptFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}