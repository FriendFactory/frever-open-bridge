using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    public sealed class DataEncryptionHelper
    {
        private const string KEY = "FE56F6HG90AS2VG6H8KF2DAF1CC007TY";
        private const string IV = "01SD6CC5J9LI55II";
        
        public async Task<string> Encrypt(string input)
        {
            var aesCryptoServiceProvider = GetCryptoServiceProvider();
            var encryptor = aesCryptoServiceProvider.CreateEncryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV);

            byte[] result;
            
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        await swEncrypt.WriteAsync(input);
                    }
                    
                    result = msEncrypt.ToArray();
                }
            }
            
            return Convert.ToBase64String(result);
        }
        

        public string Decrypt(string inputData)
        {
            var aesCryptoServiceProvider = GetCryptoServiceProvider();
            var decryptor = aesCryptoServiceProvider.CreateDecryptor();
 
            var byteData = Convert.FromBase64String(inputData);
            var result = decryptor.TransformFinalBlock(byteData, 0, byteData.Length);
            
            return Encoding.ASCII.GetString(result);
        }

        private AesCryptoServiceProvider GetCryptoServiceProvider()
        {
            var aesCryptoServiceProvider = new AesCryptoServiceProvider();
            aesCryptoServiceProvider.BlockSize = 128;
            aesCryptoServiceProvider.KeySize = 256;
            aesCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(KEY);
            aesCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(IV);
            aesCryptoServiceProvider.Mode = CipherMode.CBC;
            aesCryptoServiceProvider.Padding = PaddingMode.PKCS7;
            
            return aesCryptoServiceProvider;
        }
 
    }
}
