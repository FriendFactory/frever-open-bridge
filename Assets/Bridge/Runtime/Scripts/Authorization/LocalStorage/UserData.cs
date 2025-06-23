using System;

namespace Bridge.Authorization.LocalStorage
{
    [Serializable]
    public class UserData
    {
        public FFEnvironment FfEnvironment;
        public AuthToken Token;
    }
}
