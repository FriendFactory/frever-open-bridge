using System.Security.Cryptography;
using System.Text;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal sealed class CryptoServiceProvider
    {
        private const string KEY = "FE56F6HG90AS2VG6H8KF2DAF1CC007TY";
        private const string IV = "01SD6CC5J9LI55II";

        public AesCryptoServiceProvider GetProvider() => new AesCryptoServiceProvider
        {
            BlockSize = Constants.BLOCK_SIZE,
            KeySize = 256,
            Key = Encoding.ASCII.GetBytes(KEY),
            IV = Encoding.ASCII.GetBytes(IV),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        
        public AesCryptoServiceProvider GetPartialEncryptionProvider() => new AesCryptoServiceProvider
        {
            BlockSize = Constants.BLOCK_SIZE,
            KeySize = 256,
            Key = Encoding.ASCII.GetBytes(KEY),
            IV = Encoding.ASCII.GetBytes(IV),
            Mode = CipherMode.CBC,
            Padding = PaddingMode.None
        };
    }
}