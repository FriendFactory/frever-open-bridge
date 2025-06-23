using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Bridge.Authorization.LocalStorage
{
    internal sealed class EncryptedUserDataHandler : IUserDataHandler
    {
        private readonly DataEncryptionHelper _encryptionHelper = new DataEncryptionHelper();
        private static string FileName => Constants.FileNameConstants.AUTH_FILE_NAME;
        
        private string StoredEncryptedTokenPath => $"{Application.persistentDataPath}/{FileName}";
        public bool HasSavedFile => File.Exists(StoredEncryptedTokenPath);
        
        public UserData ReadFile()
        {
            var textData = File.ReadAllText(StoredEncryptedTokenPath);
            var data = _encryptionHelper.Decrypt(textData);
            return JsonConvert.DeserializeObject<UserData>(data);
        }

        public async void SaveFile(UserData data)
        {
            var json = JsonConvert.SerializeObject(data);
            var encryptedData = await _encryptionHelper.Encrypt(json);
            
            using (var writer = File.CreateText(StoredEncryptedTokenPath))
            {
                await writer.WriteAsync(encryptedData);
            }
        }

        public void DeleteFile()
        {
            if (!HasSavedFile) return;
            
            File.Delete(StoredEncryptedTokenPath);
        }
    }
}