using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Exceptions;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal sealed class EncryptedFileWriter: IEncryptedFileWriter 
    {
        private readonly CryptoServiceProvider _cryptoServiceProvider;
        
        public EncryptedFileWriter(CryptoServiceProvider cryptoServiceProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
        }

        public async Task CopyLocalFileAsync(string filePath, string sourceFile, CancellationToken cancellationToken)
        {
            try
            {
                using (var sourceStream = File.OpenRead(sourceFile))
                using (var encryptedStream = File.Create(filePath))
                {
                    // do not apply encryption to already encrypted file
                    if (sourceFile.EndsWith(Constants.ENCRYPTED_FILE_EXTENSION))
                    {
                        await sourceStream.CopyToAsync(encryptedStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);
                        return;
                    }

                    using (var aes = _cryptoServiceProvider.GetProvider())
                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                    {
                        await sourceStream.CopyToAsync(cryptoStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);
                    }
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Encryption failed: {e.Message}", e);
            }
        }

        public async Task WriteFileFromStreamAsync(string filePath, Stream sourceStream, CancellationToken cancellationToken)
        {
            try
            {
                using (var encryptedStream = File.Create(filePath))
                using (var aes = _cryptoServiceProvider.GetProvider())
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    await sourceStream.CopyToAsync(cryptoStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Encryption failed: {e.Message}", e);
            }
        }

        public async Task WriteFileBytesAsync(string filePath, byte[] bytes, CancellationToken cancellationToken)
        {
            try
            {
                using (var sourceStream = new MemoryStream(bytes))
                using (var encryptedStream = File.Create(filePath))
                using (var aes = _cryptoServiceProvider.GetProvider())
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    await sourceStream.CopyToAsync(cryptoStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Encryption failed: {e.Message}", e);
            }
        }
    }
}