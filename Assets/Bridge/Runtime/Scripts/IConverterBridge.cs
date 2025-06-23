using System.IO;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge
{
    public interface IConverterBridge
    {
        Task<ExtractingAudioResult> ExtractAudioAsync(byte[] videoBytes, int durationSec);
        Task<ConvertingResult> ConvertAsync(Stream fileStream, string filePath, string fileExtension);
        Task CleanCacheFromConvertedFilesAsync();
    }
}