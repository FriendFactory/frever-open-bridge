using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal interface IEncryptedFileReader
    {
        Task<byte[]> DecryptFileToMemoryAsync(string filePath, CancellationToken cancellationToken = default);
        Task<MemoryStream> DecryptFileToMemoryStreamAsync(string filePath, CancellationToken cancellationToken = default);
    }
}