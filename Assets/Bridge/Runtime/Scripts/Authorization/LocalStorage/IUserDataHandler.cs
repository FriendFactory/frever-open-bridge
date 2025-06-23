namespace Bridge.Authorization.LocalStorage
{
    internal interface IUserDataHandler
    {
        bool HasSavedFile { get;}
        UserData ReadFile();
        void SaveFile(UserData data);
        void DeleteFile();
    }
}