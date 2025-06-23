using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Services.AssetService.Caching
{
    internal class FileWriter: IFileWriter
    {
        private const int DEFAULT_BUFFER_SIZE = 81920;
        
        public async Task WriteFileBytesAsync(string filePath, byte[] bytes, CancellationToken cancellationToken)
        {
            using (var file = File.Create(filePath))
            {
                await file.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            }
        }

        public async Task CopyLocalFileAsync(string filePath, string sourceFile, CancellationToken cancellationToken)
        {
            using (var stream = File.OpenRead(sourceFile))
            {
                using (var file = File.Create(filePath))
                {
                    await stream.CopyToAsync(file, DEFAULT_BUFFER_SIZE, cancellationToken);
                }
            }
        }

        public async Task WriteFileFromStreamAsync(string filePath, Stream sourceStream, CancellationToken cancellationToken)
        {
            using (var file = File.Create(filePath))
            {
                await sourceStream.CopyToAsync(file, DEFAULT_BUFFER_SIZE, cancellationToken);
            }
        }
    }
}