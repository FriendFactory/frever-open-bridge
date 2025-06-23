namespace Bridge.Authorization.LocalStorage
{
    namespace Storage
    {
        internal class UserDataStorage : IUserDataStorage
        {
            private readonly JsonUserDataHandler _jsonUserDataHandler = new JsonUserDataHandler();
            private readonly EncryptedUserDataHandler _encryptedUserDataHandler = new EncryptedUserDataHandler();
            
            public UserData UserData { get; private set; }
            public bool HasSavedData => _encryptedUserDataHandler.HasSavedFile || _jsonUserDataHandler.HasSavedFile;

            public void Clear()
            {
                UserData = null;
                _encryptedUserDataHandler.DeleteFile();
                _jsonUserDataHandler.DeleteFile();
            }

            public void Load()
            {
                if (_encryptedUserDataHandler.HasSavedFile)
                {
                    UserData = _encryptedUserDataHandler.ReadFile();
                }
                else if (_jsonUserDataHandler.HasSavedFile)
                {
                    UserData = _jsonUserDataHandler.ReadFile();
                }
            }

            public void Save(UserData userData)
            {
                UserData = userData;
                _encryptedUserDataHandler.SaveFile(userData);
                _jsonUserDataHandler.DeleteFile();
            }
        }
    } 
}