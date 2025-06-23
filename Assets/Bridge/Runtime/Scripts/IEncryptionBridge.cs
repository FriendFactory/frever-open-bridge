using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge
{
    public interface IEncryptionBridge
    {
        bool EncryptionEnabled { get; }
        string TargetExtension { get; }
        Task DecryptFileAsync(string filePath, CancellationToken cancellationToken = default);
        Task<byte[]> DecryptFileToMemoryAsync(string filePath, CancellationToken cancellationToken = default);
        Task<MemoryStream> DecryptFileToMemoryStreamAsync(string filePath, CancellationToken cancellationToken = default);
        Task EncryptFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}