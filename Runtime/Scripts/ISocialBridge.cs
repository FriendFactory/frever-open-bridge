using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Results;
using Bridge.Services.UserProfile;
using Bridge.Services.UserProfile.PhoneLookup;


namespace Bridge
{
    public interface ISocialBridge: IBlockUserService
    {
        Task<FriendsByPhoneLookupResult> LookupForFriends(string[] phoneNumbers);
        Task<ProfileResult<MyProfile>> GetCurrentUserInfo(CancellationToken cancellationToken = default);
        Task<ProfileResult<Profile>> GetProfile(long groupId, CancellationToken cancellationToken = default);
        Task<ProfileResult<Profile>> GetMyProfile(CancellationToken cancellationToken = default);
        Task<ProfileResult<PublicProfile>> GetPublicProfileFor(string nickname, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetProfiles(int take, int skip, string filterNickname = null, ProfileSorting sorting = ProfileSorting.ByLevel, bool excludeMinors = false,
            CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetMyFollowers(int take, int skip, string nickname = null, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetFollowersFor(long groupId, int take, int skip, string nickname = null, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetFollowedByCurrentUser(int take, int skip, string nickname = null, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetFollowedBy(long groupId, int take, int skip, string nickname = null, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetMyFriends(int take, int skip, string nickname = null, bool withCrewMembers = true, bool startChatOnly = false, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetFriends(long groupId, int take, int skip, string nickname = null, bool withCrewMembers = true, bool startChatOnly = false, CancellationToken cancellationToken = default);
        Task<ProfilesResult<Profile>> GetStarCreatorsInYourCountry(CancellationToken cancellationToken = default);
        Task<Result<PurchasedAssetsData>> GetPurchasedAssetsInfo(CancellationToken token = default);
        Task<Result<UserBalance>> GetUserBalance(CancellationToken token = default);
        Task<Result> ClaimWelcomeGift(CancellationToken token = default);
        string GetUserProfilePublicUrl(string userNickname);
        Task<ProfileResult<Profile>> GetFreverOfficialProfile(CancellationToken cancellationToken = default);
        Task<ProfilesResult<GroupShortInfo>> GetProfilesShortInfo(long[] groupIds, CancellationToken token = default);
        Task<Result> UpdateUserProfile(UpdateProfileRequest model);
        Task<Result> UpdateCharacterAccess(CharacterAccess characterAccess);
        Task<Result> CompleteOnboarding();
        Task<Result> UpdateUserMainCharacter(long characterId, long universeId);
        Task<Result> UpdateUserCountry(long country);
        Task<Result> UpdateUserGender(long gender);
        Task<Result> UpdateUserBio(string bio);
        Task<Result> UpdateUserBioLinks(Dictionary<string, string> links);
        Task<ProfileResult<Profile>> StartFollow(long groupId);
        Task<Result> StopFollow(long groupId);
        Task<Result> DeleteMyAccount();
        Task<Result<StarCreator>> SubmitSupportCreatorCode(string text);
        Task<Result> CancelSupportCreator();
        Task<Result> UpdateOnlineStatus();
    }
}