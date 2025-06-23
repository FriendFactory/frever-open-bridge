using System.IO;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.Services.TranscodingService
{
    internal interface ITranscodingService
    {
        Task<ExtractingAudioResult> ExtractAudioAsync(byte[] bytes, int durationSec);
        Task<ConvertingResult> ConvertAsync(Stream fileStream, string filePath, string fileExtension);
        Task CleanCacheFromConvertedFiles();
    }
}