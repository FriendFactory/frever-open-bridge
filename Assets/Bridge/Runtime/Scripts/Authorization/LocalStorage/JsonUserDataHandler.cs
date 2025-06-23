using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Bridge.Authorization.LocalStorage
{
    internal sealed class JsonUserDataHandler : IUserDataHandler
    {
        private string StoredJsonTokenPath => Application.persistentDataPath + "/auth_data";
        public bool HasSavedFile => File.Exists(StoredJsonTokenPath);

        public UserData ReadFile()
        {
            var json = File.ReadAllText(StoredJsonTokenPath);
            return JsonConvert.DeserializeObject<UserData>(json);
        }

        public void SaveFile(UserData data)
        {
            throw new InvalidOperationException("Writing save file to json is obsolete. Please use encrypted.");
        }

        public void DeleteFile()
        {
            if (!HasSavedFile) return;
            
            File.Delete(StoredJsonTokenPath);
        }
    }
}