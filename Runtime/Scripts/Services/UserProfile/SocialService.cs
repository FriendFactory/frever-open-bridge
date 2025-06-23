using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.UserProfile.PhoneLookup;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bridge.Services.UserProfile
{
    internal sealed class SocialService : ISocialService
    {
        private readonly IFriendsLookupService _friendsLookupService;
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;
        private readonly string _serviceUrl;

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public SocialService(string serviceUrl, IRequestHelper requestHelper, ISerializer serializer)
        {
            _serviceUrl = serviceUrl;
            _requestHelper = requestHelper;
            _friendsLookupService = new FriendsLookupService(serviceUrl, requestHelper, serializer);
            _serializer = serializer;
        }

        public async Task<ProfileResult<MyProfile>> GetCurrentUserProfile(CancellationToken cancellationToken)
        {
            try
            {
                return await CurrentUserProfileInternal(cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfileResult<MyProfile>.CanceledInstance() : new ProfileResult<MyProfile>(e.Message);
            }
        }

        public async Task<ProfileResult<PublicProfile>> GetPublicProfile(string nickname, CancellationToken cancellationToken)
        {
            try
            {
                return await GetPublicProfileInternal(nickname, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfileResult<PublicProfile>.CanceledInstance() : new ProfileResult<PublicProfile>(e.Message);
            }
        }

        public async Task<ProfileResult<Profile>> GetProfile(long groupId, CancellationToken cancellationToken)
        {
            try
            {
                return await GetBaseProfileInternal(groupId, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfileResult<Profile>.CanceledInstance() : new ProfileResult<Profile>(e.Message);
            }
        }

        public async Task<ProfilesResult<Profile>> GetFollowersFor(long groupId, int take, int skip, string nickname, CancellationToken cancellationToken)
        {
            try
            {
                var endPoint = $"group/{groupId}/follower?take={take}&skip={skip}";
                AddNicknameParameterIfNeeded(ref endPoint, nickname);
                var url = GetUrl(endPoint);
                return await GetProfiles(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfilesResult<Profile>.CanceledInstance() : new ProfilesResult<Profile>(e.Message);
            }
        }

        public async Task<ProfilesResult<Profile>> GetFollowedBy(long groupId, int take, int skip, string nickname, CancellationToken cancellationToken)
        {
            try
            {
                var endPoint = $"group/{groupId}/following?take={take}&skip={skip}";
                AddNicknameParameterIfNeeded(ref endPoint, nickname);
                var url = GetUrl(endPoint);
                return await GetProfiles(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfilesResult<Profile>.CanceledInstance() : new ProfilesResult<Profile>(e.Message);
            }
        }

        public async Task<ProfilesResult<Profile>> GetFriends(long groupId, int take, int skip, string nickname, bool withCrewMembers, bool startChatOnly, CancellationToken cancellationToken)
        {
            try
            {
                var endPoint = $"group/{groupId}/friends?take={take}&skip={skip}&withCrewMembers={withCrewMembers}&startChatOnly={startChatOnly}";
                AddNicknameParameterIfNeeded(ref endPoint, nickname);
                var url = GetUrl(endPoint);
                return await GetProfiles(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfilesResult<Profile>.CanceledInstance() : new ProfilesResult<Profile>(e.Message);
            }
        }

        public async Task<ProfilesResult<Profile>> GetProfiles(int take, int skip, string filterNickname,
            ProfileSorting sorting, bool excludeMinors, CancellationToken cancellationToken)
        {
            try
            {
                return await GetProfilesInternal(take, skip, filterNickname, sorting, excludeMinors, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfilesResult<Profile>.CanceledInstance() : new ProfilesResult<Profile>(e.Message);
            }
        }

        public async Task<ProfilesResult<Profile>> GetStarCreatorsInYourCountry(CancellationToken cancellationToken)
        {
            try
            {
                var url = GetUrl($"group/star-creators-in-your-country");
                return await GetProfiles(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ProfilesResult<Profile>.CanceledInstance() : new ProfilesResult<Profile>(e.Message);
            }
        }

        public async Task<Result<PurchasedAssetsData>> GetPurchasedAssetsInfo(CancellationToken token)
        {
            try
            {
                return await GetPurchasedAssetsInfoInternal(token);
            }
            catch (OperationCanceledException)
            {
                return Result<PurchasedAssetsData>.Cancelled();
            }
        }

        public async Task<Result<UserBalance>> GetUserBalance(CancellationToken token)
        {
            try
            {
                var url = Extensions.CombineUrls(_serviceUrl, "me/balance");
                var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
                var resp = await req.GetHTTPResponseAsync(token);
                if (!resp.IsSuccess)
                {
                    return Result<UserBalance>.Error(resp.DataAsText);
                }

                var model = _serializer.DeserializeProtobuf<UserBalance>(resp.Data);
                return Result<UserBalance>.Success(model);
            }
            catch (OperationCanceledException)
            {
                return Result<UserBalance>.Cancelled();
            }
        }

        public async Task<Result> ClaimWelcomeGift(CancellationToken token)
        {
            try
            {
                var url = Extensions.CombineUrls(_serviceUrl, "me/balance/initial");
                var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
                var resp = await req.GetHTTPResponseAsync(token);
                if (!resp.IsSuccess)
                {
                    return new ErrorResult(resp.DataAsText);
                }

                return new SuccessResult();
            }
            catch (OperationCanceledException)
            {
                return new CanceledResult();
            }
        }

        public string GetUserProfilePublicUrl(FFEnvironment environment, string profileNickname)
        {
            return BuildShareProfileUrl(environment, profileNickname);
        }

        public async Task<ProfileResult<Profile>> GetFreverOfficialProfile(CancellationToken cancellationToken)
        {
            try
            {
                var url = GetUrl("group/frever-official");
                return await GetProfileInternal<Profile>(url, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ProfileResult<Profile>.CanceledInstance();
            }
        }

        public async Task<ProfilesResult<GroupShortInfo>> GetProfilesShortInfo(long[] groupIds, CancellationToken token)
        {
            try
            {
                var url = GetUrl("group");
                var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
                req.AddJsonContent(_serializer.SerializeToJson(groupIds));
                var resp = await req.GetHTTPResponseAsync(token);
                if (token.IsCancellationRequested)
                {
                    return ProfilesResult<GroupShortInfo>.CanceledInstance();
                }
                
                if (!resp.IsSuccess)
                {
                    return new ProfilesResult<GroupShortInfo>(resp.DataAsText, resp.StatusCode);
                }

                var models = _serializer.DeserializeProtobuf<GroupShortInfo[]>(resp.Data);
                return new ProfilesResult<GroupShortInfo>(models);
            }
            catch (OperationCanceledException)
            {
                return ProfilesResult<GroupShortInfo>.CanceledInstance();
            }
        }

        public Task<Result> UpdateProfile(UpdateProfileRequest model)
        {
            return SendUpdateProfileRequest(HTTPMethods.Put, model);
        }

        public Task<Result> UpdateUserMainCharacter(long characterId, long universeId)
        {
            var bodyObject = new Dictionary<string, long>(1)
            {
                {nameof(UpdateProfileRequest.MainCharacterId), characterId},
                {nameof(UpdateProfileRequest.UniverseId), universeId}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public Task<Result> UpdateUserCountry(long country)
        {
            var bodyObject = new Dictionary<string, long>(1)
            {
                {nameof(UpdateProfileRequest.CountryId), country}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public Task<Result> UpdateUserGender(long gender)
        {
            var bodyObject = new Dictionary<string, long>(1)
            {
                {nameof(UpdateProfileRequest.Gender), gender}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public Task<Result> UpdateCharacterAccess(CharacterAccess characterAccess)
        {
            var bodyObject = new Dictionary<string, CharacterAccess>(1)
            {
                {nameof(UpdateProfileRequest.CharacterAccess), characterAccess}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public Task<Result> CompleteOnboarding()
        {
            var bodyObject = new Dictionary<string, bool>(1)
            {
                {nameof(MyProfile.IsOnboardingCompleted), true}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public Task<Result> UpdateUserBio(string bio)
        {
            var bodyObject = new Dictionary<string, string>(1)
            {
                {nameof(UpdateProfileRequest.Bio), bio}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public Task<Result> UpdateUserBioLinks(Dictionary<string, string> links)
        {
            var bodyObject = new Dictionary<string, Dictionary<string, string>>(1)
            {
                {nameof(UpdateProfileRequest.BioLinks), links}
            };
            return SendUpdateProfileRequest(HTTPMethods.Patch, bodyObject);
        }

        public async Task<ProfileResult<Profile>> StartFollow(long groupId)
        {
            var url = GetUrl($"group/{groupId}/follower");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new ProfileResult<Profile>(resp.DataAsText, resp.StatusCode);

            var profile = _serializer.DeserializeProtobuf<Profile>(resp.Data);
            return new ProfileResult<Profile>(profile);
        }

        public async Task<Result> StopFollow(long groupId)
        {
            var url = GetUrl($"group/{groupId}/follower");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new ErrorResult(resp.DataAsText, resp.StatusCode);

            return new SuccessResult();
        }

        public async Task<Result> DeleteMyAccount()
        {
            var url = Extensions.CombineUrls(_serviceUrl, "me");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            using (var resp = await req.GetHTTPResponseAsync())
            {
                if (!resp.IsSuccess)
                    return new ErrorResult(resp.DataAsText, resp.StatusCode);
                
                return new SuccessResult();
            }
        }

        public async Task<Result<StarCreator>> SubmitSupportCreatorCode(string creatorCode)
        {
            var url = Extensions.CombineUrls(_serviceUrl, $"creator-code/usage/{creatorCode}");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await req.GetHTTPResponseAsync();
            
            if (!resp.IsSuccess)
            {
                return Result<StarCreator>.Error(resp.DataAsText, resp.StatusCode);
            }

            return Result<StarCreator>.Success(_serializer.DeserializeJson<StarCreator>(resp.DataAsText));
        }

        public async Task<Result> CancelSupportCreator()
        {
            var url = Extensions.CombineUrls(_serviceUrl, $"creator-code/usage");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            var resp = await req.GetHTTPResponseAsync();
            
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText, resp.StatusCode);
            }

            return new SuccessResult();
        }

        public async Task<Result> UpdateOnlineStatus()
        {
            var url = Extensions.CombineUrls(_serviceUrl, "me/status/online");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
            var resp = await req.GetHTTPResponseAsync();

            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText, resp.StatusCode);
            }

            return new SuccessResult();
        }

        public Task<FriendsByPhoneLookupResult> LookupForFriends(string[] phoneNumbers)
        {
            return _friendsLookupService.LookupForFriends(phoneNumbers);
        }

        private Task<ProfilesResult<Profile>> GetProfilesInternal(int take, int skip, string nickNameFilter,
            ProfileSorting sorting, bool excludeMinors, CancellationToken cancellationToken)
        {
            var endpoint = $"group/top?count={take}&skip={skip}&sorting={sorting}&excludeMinors={excludeMinors}";
            if (!string.IsNullOrEmpty(nickNameFilter))
            {
                endpoint += $"&nickname={nickNameFilter}";
            }
            var url = GetUrl(endpoint);
            return GetProfiles(url, cancellationToken);
        }
        
        private Task<ProfileResult<Profile>> GetBaseProfileInternal(long groupId, CancellationToken cancellationToken)
        {
            var url = GetUrl($"group/{groupId}");
            return GetProfileInternal<Profile>(url, cancellationToken);
        }
        
        private Task<ProfileResult<PublicProfile>> GetPublicProfileInternal(string nickname, CancellationToken cancellationToken)
        {
            var url = GetUrl($"group/public/{nickname}");
            return GetProfileInternal<PublicProfile>(url, cancellationToken);
        }

        private async Task<ProfileResult<T>> GetProfileInternal<T>(Uri url, CancellationToken cancellationToken)
        {
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);

            if (!resp.IsSuccess)
                return new ProfileResult<T>(resp.DataAsText, resp.StatusCode);

            var profile = _serializer.DeserializeProtobuf<T>(resp.Data);
            return new ProfileResult<T>(profile);
        }

        private async Task<ProfilesResult<Profile>> GetProfiles(Uri apiUrl, CancellationToken cancellationToken)
        {
            var req = _requestHelper.CreateRequest(apiUrl, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            
            if (!resp.IsSuccess)
                return new ProfilesResult<Profile>(resp.DataAsText, resp.StatusCode);
            
            var profiles = _serializer.DeserializeProtobuf<Profile[]>(resp.Data);
            return new ProfilesResult<Profile>(profiles);
        }
        
        private async Task<ProfileResult<MyProfile>> CurrentUserProfileInternal(CancellationToken cancellationToken)
        {
            var url = GetUrl("me");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            if (!resp.IsSuccess)
                return new ProfileResult<MyProfile>(resp.DataAsText, resp.StatusCode);

            var userInfo = _serializer.DeserializeProtobuf<MyProfile>(resp.Data);
            return new ProfileResult<MyProfile>(userInfo);
        }

        private Uri GetUrl(string relativePath)
        {
            var url = new Uri(_serviceUrl);
            return new Uri(url, relativePath);
        }
        
        private async Task<Result> SendUpdateProfileRequest(HTTPMethods method, object body)
        {
            var url = Extensions.CombineUrls(_serviceUrl, "me");
            var req = _requestHelper.CreateRequest(url, method, true, false);
            var bodyJson = _serializer.SerializeToJson(body, _serializerSettings);
            req.AddJsonContent(bodyJson);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
                return new SuccessResult();
            return new ErrorResult($"{resp.StatusCode}:{resp.DataAsText}");
        }

        public async Task<ProfilesResult<Profile>> GetBlockedProfiles(CancellationToken cancellationToken)
        {
            try
            {
                return await GetBlockedProfilesInternal(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ProfilesResult<Profile>.CanceledInstance();
            }
        }

        public Task<Result> BlockUser(long groupIdToBlock)
        {
            return SendBlockControllerRequest("block", groupIdToBlock);
        }

        public Task<Result> UnBlockUser(long groupIdToUnblock)
        {
            return SendBlockControllerRequest("unblock", groupIdToUnblock);
        }

        private async Task<ProfilesResult<Profile>> GetBlockedProfilesInternal(CancellationToken cancellationToken)
        {
            var url = GetUrl("group/blocked-users");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            if (!resp.IsSuccess)
            {
                return new ProfilesResult<Profile>(resp.DataAsText, resp.StatusCode);
            }
            var blockedUsers = _serializer.DeserializeProtobuf<Profile[]>(resp.Data);
            return new ProfilesResult<Profile>(blockedUsers);
        }

        private async Task<Result> SendBlockControllerRequest(string actionName, long groupIdToBlock)
        {
            var url = GetUrl($"group/{actionName}/{groupIdToBlock}");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
            {
                return new SuccessResult();
            }
            return new ErrorResult(resp.DataAsText);
        }

        private async Task<Result<PurchasedAssetsData>> GetPurchasedAssetsInfoInternal(CancellationToken token)
        {
            var url = Extensions.CombineUrls(_serviceUrl, "me/PurchasedAssetsInfo");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess)
            {
                return Result<PurchasedAssetsData>.Error(resp.DataAsText, resp.StatusCode);
            }

            var model = _serializer.DeserializeProtobuf<PurchasedAssetsData>(resp.Data);
            return Result<PurchasedAssetsData>.Success(model);
        }

        private string BuildShareProfileUrl(FFEnvironment environment, string nickname)
        {
            var url = $"https://web.frever-api.com/@{nickname}";
            if (environment == FFEnvironment.Production || environment == FFEnvironment.ProductionUSA) return url;
            
            string envKey;
            switch (environment)
            {
                case FFEnvironment.Stage:
                    envKey = "stage";
                    break;
                case FFEnvironment.Test:
                    envKey = "test";
                    break;
                case FFEnvironment.Develop:
                    envKey = "dev";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(environment), environment, null);
            }
            
            return $"{url}/{envKey}";
        }

        private static void AddNicknameParameterIfNeeded(ref string url, string nickname)
        {
            if(string.IsNullOrEmpty(nickname)) return;
            url += $"&nickname={nickname}";
        }
    }
}