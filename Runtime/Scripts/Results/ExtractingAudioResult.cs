using UnityEngine;

namespace Bridge.Results
{
    public sealed class ExtractingAudioResult: Result
    {
        public readonly AudioClip AudioClip;
        public readonly string FilePath;
        public readonly string CopyrightsCheckInfo;

        internal ExtractingAudioResult(AudioClip audioClip, string filePath, string copyrightsCheckInfo)
        {
            AudioClip = audioClip;
            FilePath = filePath;
            CopyrightsCheckInfo = copyrightsCheckInfo;
        }

        internal ExtractingAudioResult(string errorMessage) : base(errorMessage)
        {
        }
    }
}