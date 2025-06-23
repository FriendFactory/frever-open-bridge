using System.Threading.Tasks;

namespace Bridge.Services.UserProfile.PhoneLookup
{
    internal interface IFriendsLookupService
    {
        Task<FriendsByPhoneLookupResult> LookupForFriends(string[] phoneNumbers);
    }
}
