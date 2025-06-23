using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Exceptions;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal sealed class PartialEncryptedFileWriter: IPartialEncryptedFileWriter 
    {
        private readonly CryptoServiceProvider _cryptoServiceProvider;

        public PartialEncryptedFileWriter(CryptoServiceProvider cryptoServiceProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
        }

        public async Task WriteFileBytesAsync(string filePath, byte[] bytes, CancellationToken cancellationToken)
        {
            try
            {
                using (var encryptedStream = File.Create(filePath))
                {
                    var length = bytes.Length;
                    var blockLength = Constants.GetEncryptionBlockLength(length);
                    var blockToEncrypt = new byte[blockLength];

                    Array.Copy(bytes, blockToEncrypt, blockLength);

                    var encryptedBlock = await GetEncryptedBlockAsync(blockToEncrypt, cancellationToken);

                    await encryptedStream.WriteAsync(bytes, 0, length, cancellationToken);
                    encryptedStream.Seek(0, SeekOrigin.Begin);
                    await encryptedStream.WriteAsync(encryptedBlock, 0, encryptedBlock.Length, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Partial encryption failed: {e.Message}", e);
            }
        }

        public async Task CopyLocalFileAsync(string filePath, string sourceFile, CancellationToken cancellationToken)
        {
            try
            {
                using (var sourceStream = File.OpenRead(sourceFile))
                using (var encryptedStream = File.Create(filePath))
                {
                    await sourceStream.CopyToAsync(encryptedStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);

                    // do not apply encryption to already encrypted file
                    if (sourceFile.EndsWith(Constants.ENCRYPTED_FILE_EXTENSION)) return;

                    var length = (int)sourceStream.Length;
                    var blockLength = Constants.GetEncryptionBlockLength(length);
                    var blockToEncrypt = new byte[blockLength];

                    encryptedStream.Seek(0, SeekOrigin.Begin);
                    await encryptedStream.ReadAsync(blockToEncrypt, 0, blockLength, cancellationToken);

                    var encryptedBlock = await GetEncryptedBlockAsync(blockToEncrypt, cancellationToken);

                    encryptedStream.Seek(0, SeekOrigin.Begin);
                    await encryptedStream.WriteAsync(encryptedBlock, 0, encryptedBlock.Length, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Partial encryption failed: {e.Message}", e);
            }
        }

        public async Task WriteFileFromStreamAsync(string filePath, Stream sourceStream, CancellationToken cancellationToken)
        {
            try
            {
                using (var encryptedStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    await sourceStream.CopyToAsync(encryptedStream);

                    var length = (int)sourceStream.Length;
                    var blockLength = Constants.GetEncryptionBlockLength(length);
                    var blockToEncrypt = new byte[blockLength];

                    encryptedStream.Seek(0, SeekOrigin.Begin);
                    await encryptedStream.ReadAsync(blockToEncrypt, 0, blockLength);

                    var encryptedBlock = await GetEncryptedBlockAsync(blockToEncrypt, cancellationToken);

                    encryptedStream.Seek(0, SeekOrigin.Begin);
                    await encryptedStream.WriteAsync(encryptedBlock, 0, encryptedBlock.Length, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Partial encryption failed: {e.Message}", e);
            }
        }

        public async Task EncryptFileAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                using (var encryptedStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var length = (int)encryptedStream.Length;
                    var blockLength = Constants.GetEncryptionBlockLength(length);
                    var blockToEncrypt = new byte[blockLength];

                    await encryptedStream.ReadAsync(blockToEncrypt, 0, blockLength, cancellationToken);

                    var encryptedBlock = await GetEncryptedBlockAsync(blockToEncrypt, cancellationToken);

                    encryptedStream.Seek(0, SeekOrigin.Begin);
                    await encryptedStream.WriteAsync(encryptedBlock, 0, encryptedBlock.Length, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Partial encryption failed: {e.Message}", e);
            }
        }
        
        private async Task<byte[]> GetEncryptedBlockAsync(byte[] blockToEncrypt, CancellationToken cancellationToken)
        {
            var blockLength = blockToEncrypt.Length;
            
            using (var aes = _cryptoServiceProvider.GetPartialEncryptionProvider())
            using(var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var encryptedBlockStream = new MemoryStream(blockLength))
            using (var cryptoStream = new CryptoStream(encryptedBlockStream, encryptor, CryptoStreamMode.Write))
            {
                await cryptoStream.WriteAsync(blockToEncrypt, 0, blockLength, cancellationToken);

                return encryptedBlockStream.ToArray();
            }
        }
    }
}