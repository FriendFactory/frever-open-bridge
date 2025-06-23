using System;

namespace Bridge.Authorization.Models
{
    [Serializable]
    public struct UserInfo
    {
        public long UserId;
        public long PrimaryGroupId;
        public long[] GroupIds;
        public string Email;
        public bool Employee;
        public bool QA;
        public bool Moderator;
        public bool Artist;
        public bool RegisteredWithAppleId;
        public bool OnboardingCompleted;
        public long? MainCharacterId;
    }
}