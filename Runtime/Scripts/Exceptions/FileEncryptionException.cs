using System;

namespace Bridge.Exceptions
{
    public class FileEncryptionException: Exception
    {
        public static string ERROR_PREFIX = "[ENCRYPTION_ERROR]";
        
        public FileEncryptionException() { }
        public FileEncryptionException(string message) : base(message) { }
        public FileEncryptionException(string message, Exception innerException) : base(message, innerException) { }

        public static string BuildErrorMessage(string message) => $"{ERROR_PREFIX} {message}";
    }
}