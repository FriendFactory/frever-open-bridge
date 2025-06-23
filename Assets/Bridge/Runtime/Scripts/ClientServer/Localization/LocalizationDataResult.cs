using Bridge.Results;

namespace Bridge.ClientServer.Localization
{
    public sealed class LocalizationDataResult: Result
    {
        public readonly string LastVersionFilePath;
        /// <summary>
        /// In case of failing getting latest actual version from the backend, use last stored locally
        /// </summary>
        public readonly string LastCachedFilePath;

        private LocalizationDataResult(string lastVersionFilePath)
        {
            LastVersionFilePath = lastVersionFilePath;
        }
        
        private LocalizationDataResult(string error, int statusCode, string lastCachePath):base(error, statusCode)
        {
            LastCachedFilePath = lastCachePath;
        }

        private LocalizationDataResult()
        {
        }

        internal static LocalizationDataResult Success(string filePath)
        {
            return new LocalizationDataResult(filePath);
        }

        internal static LocalizationDataResult Failed(string reason, int statusCode, string lastCachedFilePath)
        {
            return new LocalizationDataResult(reason, statusCode, lastCachedFilePath);
        }

        internal static LocalizationDataResult Cancelled()
        {
            return new LocalizationDataResult()
            {
                IsRequestCanceled = true
            };
        }
    }
}