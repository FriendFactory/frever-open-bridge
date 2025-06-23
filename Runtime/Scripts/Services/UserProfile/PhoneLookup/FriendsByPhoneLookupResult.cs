using Bridge.Results;

namespace Bridge.Services.UserProfile.PhoneLookup
{
    public sealed class FriendsByPhoneLookupResult: Result
    {
        public PhoneLookupInfo[] MatchedProfiles;

        internal FriendsByPhoneLookupResult(PhoneLookupInfo[] matchedProfiles)
        {
            MatchedProfiles = matchedProfiles;
        }
        
        internal FriendsByPhoneLookupResult(string error):base(error)
        {
        }
    }
}