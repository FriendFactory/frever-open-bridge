using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Results;
using Bridge.Services.UserProfile.PhoneLookup;

namespace Bridge.Services.UserProfile
{
    internal interface ISocialService: IFriendsLookupService, IBlockUserService
    {
        Task<ProfileResult<MyProfile>> GetCurrentUserProfile(CancellationToken cancellationToken);
        Task<ProfileResult<PublicProfile>> GetPublicProfile(string nickname, CancellationToken cancellationToken);
        Task<ProfileResult<Profile>> GetProfile(long groupId, CancellationToken cancellationToken);
        Task<ProfilesResult<Profile>> GetFollowersFor(long groupId, int take, int skip, string nickname, CancellationToken cancellationToken);
        Task<ProfilesResult<Profile>> GetFollowedBy(long groupId, int take, int skip, string nickname, CancellationToken cancellationToken);
        Task<ProfilesResult<Profile>> GetFriends(long groupId, int take, int skip, string nickname, bool withCrewMembers, bool startChatOnly, CancellationToken cancellationToken);
        Task<ProfilesResult<Profile>> GetProfiles(int take, int skip, string filterNickname, ProfileSorting sorting,
            bool excludeMinors, CancellationToken cancellationToken);
        Task<ProfilesResult<Profile>> GetStarCreatorsInYourCountry(CancellationToken cancellationToken);
        Task<Result<PurchasedAssetsData>> GetPurchasedAssetsInfo(CancellationToken token);
        Task<Result<UserBalance>> GetUserBalance(CancellationToken token);
        Task<Result> ClaimWelcomeGift(CancellationToken token);
        string GetUserProfilePublicUrl(FFEnvironment environment, string profileNickname);
        Task<ProfileResult<Profile>> GetFreverOfficialProfile(CancellationToken cancellationToken);
        Task<ProfilesResult<GroupShortInfo>> GetProfilesShortInfo(long[] groupIds, CancellationToken token);
        Task<Result> UpdateProfile(UpdateProfileRequest model);
        Task<Result> UpdateUserMainCharacter(long characterId, long universeId);
        Task<Result> UpdateUserCountry(long country);
        Task<Result> UpdateUserGender(long gender);
        Task<Result> UpdateCharacterAccess(CharacterAccess characterAccess);
        Task<Result> CompleteOnboarding();
        Task<Result> UpdateUserBio(string bio);
        Task<Result> UpdateUserBioLinks(Dictionary<string, string> links);
        
        Task<ProfileResult<Profile>> StartFollow(long groupId);
        Task<Result> StopFollow(long groupId);
        Task<Result> DeleteMyAccount();
        Task<Result<StarCreator>> SubmitSupportCreatorCode(string creatorCode);
        Task<Result> CancelSupportCreator();
        Task<Result> UpdateOnlineStatus();
    }

    public interface IBlockUserService
    {
        Task<ProfilesResult<Profile>> GetBlockedProfiles(CancellationToken cancellationToken = default);

        Task<Result> BlockUser(long groupIdToBlock);

        Task<Result> UnBlockUser(long groupIdToUnblock);
    }
}