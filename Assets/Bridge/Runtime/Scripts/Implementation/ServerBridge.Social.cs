using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Results;
using Bridge.Services.UserProfile;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ProfileResult<MyProfile>> GetCurrentUserInfo(CancellationToken cancellationToken)
        {
            return _socialService.GetCurrentUserProfile(cancellationToken);
        }

        public Task<ProfileResult<Profile>> GetProfile(long groupId, CancellationToken cancellationToken)
        {
            return _socialService.GetProfile(groupId, cancellationToken);
        }

        public Task<ProfileResult<Profile>> GetMyProfile(CancellationToken cancellationToken = default)
        {
            return _socialService.GetProfile(Profile.GroupId, cancellationToken);
        }

        public Task<ProfileResult<PublicProfile>> GetPublicProfileFor(string nickname, CancellationToken cancellationToken)
        {
            return _socialService.GetPublicProfile(nickname, cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetProfiles(int take, int skip, string filterNickname = null, ProfileSorting sorting = ProfileSorting.ByLevel,
            bool excludeMinors = false,
            CancellationToken cancellationToken = default)
        {
            return _socialService.GetProfiles(take, skip, filterNickname, sorting, excludeMinors, cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetMyFollowers(int take, int skip, string nickname = null, CancellationToken cancellationToken = default)
        {
            return _socialService.GetFollowersFor(Profile.GroupId, take, skip, nickname, cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetFollowersFor(long groupId, int take, int skip, string nickname = null, CancellationToken cancellationToken = default)
        {
            return _socialService.GetFollowersFor(groupId, take, skip, nickname, cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetFollowedByCurrentUser(int take, int skip, string nickname = null, CancellationToken cancellationToken = default)
        {
            return _socialService.GetFollowedBy(Profile.GroupId, take, skip, nickname, cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetFollowedBy(long groupId, int take, int skip, string nickname = null, CancellationToken cancellationToken = default)
        {
            return _socialService.GetFollowedBy(groupId, take, skip, nickname, cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetFriends(long groupId, int take, int skip, string nickname = null, bool withCrewMembers = true, bool startChatOnly = false, CancellationToken cancellationToken = default)
        {
            return _socialService.GetFriends(groupId, take, skip, nickname, withCrewMembers, startChatOnly, cancellationToken);
        }

        public Task<Result<PurchasedAssetsData>> GetPurchasedAssetsInfo(CancellationToken token)
        {
            return _socialService.GetPurchasedAssetsInfo(token);
        }

        public Task<Result<UserBalance>> GetUserBalance(CancellationToken token)
        {
            return _socialService.GetUserBalance(token);
        }

        public Task<Result> ClaimWelcomeGift(CancellationToken token)
        {
            return _socialService.ClaimWelcomeGift(token);
        }

        public string GetUserProfilePublicUrl(string nickname)
        {
            return _socialService.GetUserProfilePublicUrl(Environment, nickname);
        }

        public Task<ProfileResult<Profile>> GetFreverOfficialProfile(CancellationToken cancellationToken)
        {
            return _socialService.GetFreverOfficialProfile(cancellationToken);
        }

        public Task<ProfilesResult<GroupShortInfo>> GetProfilesShortInfo(long[] groupIds, CancellationToken token = default)
        {
            return _socialService.GetProfilesShortInfo(groupIds, token);
        }

        public Task<ProfilesResult<Profile>> GetStarCreatorsInYourCountry(CancellationToken cancellationToken = default)
        {
            return _socialService.GetStarCreatorsInYourCountry(cancellationToken);
        }

        public Task<ProfilesResult<Profile>> GetMyFriends(int take, int skip, string nickname = null, bool withCrewMembers = true, bool startChatOnly = false, CancellationToken cancellationToken = default)
        {
            return _socialService.GetFriends(Profile.GroupId, take, skip, nickname, withCrewMembers, startChatOnly, cancellationToken);
        }

        public Task<Result> UpdateUserProfile(UpdateProfileRequest model)
        {
            return _socialService.UpdateProfile(model);
        }

        public Task<Result> UpdateCharacterAccess(CharacterAccess characterAccess)
        {
            return _socialService.UpdateCharacterAccess(characterAccess);
        }

        public Task<Result> CompleteOnboarding()
        {
            return _socialService.CompleteOnboarding();
        }

        public Task<Result> UpdateUserMainCharacter(long characterId, long universeId)
        {
            return _socialService.UpdateUserMainCharacter(characterId, universeId);
        }

        public Task<Result> UpdateUserCountry(long country)
        {
            return _socialService.UpdateUserCountry(country);
        }

        public Task<Result> UpdateUserGender(long gender)
        {
            return _socialService.UpdateUserGender(gender);
        }

        public Task<Result> UpdateUserBio(string bio)
        {
            return _socialService.UpdateUserBio(bio);
        }

        public Task<Result> UpdateUserBioLinks(Dictionary<string, string> links)
        {
            return _socialService.UpdateUserBioLinks(links);
        }

        public Task<ProfileResult<Profile>> StartFollow(long groupId)
        {
            return _socialService.StartFollow(groupId);
        }

        public Task<Result> StopFollow(long groupId)
        {
            return _socialService.StopFollow(groupId);
        }

        public Task<Result> DeleteMyAccount()
        {
            return _socialService.DeleteMyAccount();
        }

        public Task<Result<StarCreator>> SubmitSupportCreatorCode(string creatorCode)
        {
            return _socialService.SubmitSupportCreatorCode(creatorCode);
        }

        public Task<Result> CancelSupportCreator()
        {
            return _socialService.CancelSupportCreator();
        }

        public Task<Result> UpdateOnlineStatus()
        {
            return _socialService.UpdateOnlineStatus();
        }

        public Task<ProfilesResult<Profile>> GetBlockedProfiles(CancellationToken cancellationToken)
        {
            return _socialService.GetBlockedProfiles(cancellationToken);
        }

        public Task<Result> BlockUser(long groupIdToBlock)
        {
            return _socialService.BlockUser(groupIdToBlock);
        }

        public Task<Result> UnBlockUser(long groupIdToUnblock)
        {
            return _socialService.UnBlockUser(groupIdToUnblock);
        }
    }
}