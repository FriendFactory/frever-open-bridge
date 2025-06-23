using System.IO;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        /// <summary>
        /// Transcode a video and receive an AudioClip of the mp3 when it is done.
        /// </summary>
        public Task<ExtractingAudioResult> ExtractAudioAsync(byte[] videoBytes, int durationSec) => _transcoder.ExtractAudioAsync(videoBytes, durationSec);

        public Task<ConvertingResult> ConvertAsync(Stream fileStream, string filePath, string fileExtension) =>
            _transcoder.ConvertAsync(fileStream, filePath, fileExtension);

        public Task CleanCacheFromConvertedFilesAsync() => _transcoder.CleanCacheFromConvertedFiles();
    }
}