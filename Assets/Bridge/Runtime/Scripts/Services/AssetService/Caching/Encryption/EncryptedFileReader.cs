using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Exceptions;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal sealed class EncryptedFileReader: IEncryptedFileReader
    {
        private readonly CryptoServiceProvider _cryptoServiceProvider;

        public EncryptedFileReader(CryptoServiceProvider cryptoServiceProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
        }

        public async Task<byte[]> DecryptFileToMemoryAsync(string filePath, CancellationToken cancellationToken)
        {
            using (var decryptedStream = await DecryptFileToMemoryStreamAsync(filePath, cancellationToken))
            {
                return decryptedStream.ToArray();
            }
        }

        public async Task<MemoryStream> DecryptFileToMemoryStreamAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                using (var encryptedStream = File.OpenRead(filePath))
                using (var aesCryptoServiceProvider = _cryptoServiceProvider.GetProvider())
                using (var decryptor = aesCryptoServiceProvider.CreateDecryptor())
                using (var cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read))
                {
                    var decryptedStream = new MemoryStream();

                    await cryptoStream.CopyToAsync(decryptedStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);
                    decryptedStream.Seek(0, SeekOrigin.Begin);

                    return decryptedStream;
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Decryption failed: {e.Message}", e);
            }
        }
    }
}