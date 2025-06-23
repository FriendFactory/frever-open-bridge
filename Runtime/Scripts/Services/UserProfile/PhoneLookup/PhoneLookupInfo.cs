using System;

namespace Bridge.Services.UserProfile.PhoneLookup
{
    public sealed class PhoneLookupInfo
    {
        public string ProvidedPhoneNumber { get; set; }

        public string FreverProfilePhone { get; set; }

        public long GroupId { get; set; }

        public string GroupNickname { get; set; }
        
        public bool IsFollowing { get; set; }
        
        public DateTime RegistrationDate { get; set; }
    }
}