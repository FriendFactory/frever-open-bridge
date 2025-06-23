using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Exceptions;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal class PartialEncryptedFileReader: IPartialEncryptedFileReader
    {
        private readonly CryptoServiceProvider _cryptoServiceProvider;
        
        public PartialEncryptedFileReader(CryptoServiceProvider cryptoServiceProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
        }

        public async Task<byte[]> DecryptFileToMemoryAsync(string filePath, CancellationToken cancellationToken = default)
        {
            using (var decryptedStream = await DecryptFileToMemoryStreamAsync(filePath, cancellationToken))
            {
                return decryptedStream.ToArray();
            }
        }

        public async Task<MemoryStream> DecryptFileToMemoryStreamAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    var decryptedStream = new MemoryStream();

                    await fileStream.CopyToAsync(decryptedStream, Constants.DEFAULT_BUFFER_SIZE, cancellationToken);

                    var length = (int)decryptedStream.Length;
                    var blockLength = Constants.GetEncryptionBlockLength(length);
                    var blockToDecrypt = new byte[blockLength];

                    decryptedStream.Seek(0, SeekOrigin.Begin);
                    await decryptedStream.ReadAsync(blockToDecrypt, 0, blockLength);

                    var decryptedBlock = await GetDecryptedBlockAsync(blockToDecrypt);

                    decryptedStream.Seek(0, SeekOrigin.Begin);
                    await decryptedStream.WriteAsync(decryptedBlock, 0, decryptedBlock.Length, cancellationToken);
                    // reset stream position in order to be properly used by consumer
                    decryptedStream.Seek(0, SeekOrigin.Begin);

                    return decryptedStream;
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Partial decryption failed: {e.Message}", e);
            }
        }

        public async Task DecryptFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var length = (int)fileStream.Length;
                    var blockLength = Constants.GetEncryptionBlockLength(length);
                    var blockToDecrypt = new byte[blockLength];

                    await fileStream.ReadAsync(blockToDecrypt, 0, blockLength, cancellationToken);

                    var decryptedBlock = await GetDecryptedBlockAsync(blockToDecrypt);

                    fileStream.Seek(0, SeekOrigin.Begin);
                    await fileStream.WriteAsync(decryptedBlock, 0, decryptedBlock.Length, cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Partial decryption failed: {e.Message}", e);
            }
        }

        private async Task<byte[]> GetDecryptedBlockAsync(byte[] blockToDecrypt)
        {
            using (var aes = _cryptoServiceProvider.GetPartialEncryptionProvider())
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var decryptedBlockStream = new MemoryStream(blockToDecrypt))
            using (var cryptoStream = new CryptoStream(decryptedBlockStream, decryptor, CryptoStreamMode.Read))
            {
                var blockLength = blockToDecrypt.Length;
                var decryptedBlock = new byte[blockLength];
                var decryptedBlockBytesRead = await cryptoStream.ReadAsync(decryptedBlock, 0, decryptedBlock.Length);
                if (decryptedBlockBytesRead != blockLength)
                {
                    Debug.LogWarning($"[{GetType().Name}] Length of decrypted blocks to read is not equal to expected block length");
                }

                return decryptedBlock;
            }
        }
    }
}