using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Services.AssetService.Caching
{
    interface IFileWriter
    {
        Task WriteFileBytesAsync(string filePath, byte[] bytes, CancellationToken cancellationToken = default);
        Task CopyLocalFileAsync(string filePath, string sourceFile, CancellationToken cancellationToken = default);
        Task WriteFileFromStreamAsync(string filePath, Stream sourceStream, CancellationToken cancellationToken = default);
    }
}