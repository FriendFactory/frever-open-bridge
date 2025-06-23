using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Services.AssetService.Caching.Encryption;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private IPartialEncryptedFileReader FileReader => _encryptionService.GetPartialEncryptedFileReader();
        private IPartialEncryptedFileWriter FileWriter => _encryptionService.GetPartialEncryptedFileWriter();

        public bool EncryptionEnabled => _encryptionService.EncryptionEnabled;
        public string TargetExtension => _encryptionService.TargetExtension;
        
        public Task DecryptFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return FileReader.DecryptFileAsync(filePath, cancellationToken);
        }

        public Task<byte[]> DecryptFileToMemoryAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return FileReader.DecryptFileToMemoryAsync(filePath, cancellationToken);
        }

        public Task<MemoryStream> DecryptFileToMemoryStreamAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return FileReader.DecryptFileToMemoryStreamAsync(filePath, cancellationToken);
        }

        public Task EncryptFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return FileWriter.EncryptFileAsync(filePath, cancellationToken);
        }
    }
}