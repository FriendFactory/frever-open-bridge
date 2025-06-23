namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal static class Constants
    {
        internal const string ENCRYPTED_FILE_EXTENSION = ".bin";
        internal const int DEFAULT_BUFFER_SIZE = 81920;
        internal const int BLOCK_SIZE = 128;
        private const int ENCRYPTED_BLOCK_LENGTH = 512 * 128;

        internal static int GetEncryptionBlockLength(int length)
        {
            if (length <= 1024) return BLOCK_SIZE;
            if (length <= 4096) return BLOCK_SIZE * 4;
            if (length <= 32768) return BLOCK_SIZE * 8;
            if (length <= ENCRYPTED_BLOCK_LENGTH) return BLOCK_SIZE * 16;

            return ENCRYPTED_BLOCK_LENGTH;
        }
    }
}