
using Bridge.Authorization.LocalStorage;

namespace Bridge.Authorization
{
    internal interface IUserDataStorage
    {
        bool HasSavedData { get; }
        UserData UserData { get;}
        void Save(UserData userData);
        void Load();
        void Clear();
    }
} 
