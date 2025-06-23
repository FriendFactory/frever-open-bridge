using System;
using System.Linq;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal sealed class AudioFileSettings : FileSettings
    {
        private static readonly FileExtension[] SupportedAudioExtensions =
            {FileExtension.Mp3, FileExtension.Ogg, FileExtension.Wav};

        private const string AUDIO_FILE_NAME = "Audio";

        public AudioFileSettings(FileExtension extension) : base(AUDIO_FILE_NAME, extension, false, FileType.MainFile, null)
        {
            if (SupportedAudioExtensions.All(x => x != extension))
            {
                throw new Exception($"Audio file can't have extension: {extension.ToString()}");
            }
        }

        public AudioFileSettings(FileExtension[] extensions) : base(AUDIO_FILE_NAME, extensions, false, FileType.MainFile, null)
        {
            if (extensions.Any(x => !SupportedAudioExtensions.Contains(x)))
            {
                throw new Exception($"Audio file can't have extension: {extensions.First(x => !SupportedAudioExtensions.Contains(x)).ToString()}");
            }
        }
    }
}