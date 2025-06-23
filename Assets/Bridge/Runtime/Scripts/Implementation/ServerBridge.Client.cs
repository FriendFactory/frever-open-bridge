using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer;
using Bridge.ClientServer.AgeControl;
using Bridge.ClientServer.Assets.BodyAnimations;
using Bridge.ClientServer.Assets.CameraFilters;
using Bridge.ClientServer.Assets.Characters;
using Bridge.ClientServer.Assets.Music;
using Bridge.ClientServer.Assets.Other;
using Bridge.ClientServer.Assets.SetLocations;
using Bridge.ClientServer.Assets.Songs;
using Bridge.ClientServer.Assets.UserSounds;
using Bridge.ClientServer.Assets.Vfxs;
using Bridge.ClientServer.Assets.Wardrobes;
using Bridge.ClientServer.EditorSettings;
using Bridge.ClientServer.Emotions;
using Bridge.ClientServer.InAppPurchases;
using Bridge.ClientServer.Level;
using Bridge.ClientServer.Localization;
using Bridge.ClientServer.Premium;
using Bridge.ClientServer.Rewards;
using Bridge.ClientServer.SocialActions;
using Bridge.ClientServer.Tasks;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.ClientServer.EditorsSetting;
using Bridge.Models.ClientServer.Gamification;
using Bridge.Models.ClientServer.Gamification.Reward;
using Bridge.Models.ClientServer.Level;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Level.Shuffle;
using Bridge.Models.ClientServer.SocialActions;
using Bridge.Models.ClientServer.StartPack;
using Bridge.Models.ClientServer.Tasks;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private ISetLocationService _setLocationService;
        private IBodyAnimationService _bodyAnimationService;
        private IVfxService _vfxService;
        private ICameraFilterService _cameraFilterService;
        private ISongService _songService;
        private IUserSoundsService _userSoundsService;
        private ICharacterService _characterService;
        private ICharacterBakingService _characterBakingService;
        private ILevelService _levelService;
        private IOutfitService _outfitService;
        private ICountryService _countryService;
        private ITaskService _taskService;
        private IRewardClaimingService _rewardClaimingService;
        private ISeasonService _seasonService;
        private IEditorSettingsService _editorSettingsService;
        private ISocialActionsService _socialActionsService;
        private IInAppPurchasesService _inAppPurchasesService;
        private IPremiumPassService _premiumPassService;
        private IRandomAssetsService _randomAssetsService;
        private IEmotionService _emotionService;
        private IMusicService _musicService;
        private IFavouriteMusicService _favouriteMusicService;
        private ILocalizationService _localizationService;
        private IAgeControlService _ageControlService;

        public Task<Result<SetLocationFullInfo>> GetSetLocationAsync(long id, CancellationToken token = default)
        {
            return _setLocationService.GetSetLocationAsync(id, token);
        }

        public Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, long? setLocationCategoryId = null, string filter = null, long? taskId = null, CancellationToken token = default)
        {
            return _setLocationService.GetSetLocationListAsync(target, takeNext, takePrevious, raceId, setLocationCategoryId, filter, taskId, token);
        }

        public Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(SetLocationFilterModel filter, CancellationToken token = default)
        {
            return _setLocationService.GetSetLocationListAsync(filter, token);
        }

        public Task<ArrayResult<SetLocationFullInfo>> GetVideoMessageSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId,
            CancellationToken token = default)
        {
            return _setLocationService.GetVideoMessageSetLocationListAsync(target, takeNext, takePrevious, raceId, token);
        }

        public Task<ArrayResult<SetLocationFullInfo>> GetMySetLocationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default)
        {
            return _setLocationService.GetMySetLocationListAsync(target, takeNext, takePrevious, token);
        }

        public Task<ArrayResult<SetLocationBackground>> GetSetLocationBackgroundListAsync(int take, int skip, CancellationToken token)
        {
            return _setLocationService.GetSetLocationBackgroundListAsync(take, skip, token);
        }

        public Task<Result<BackgroundOptions>> GetSetLocationBackgroundOptionsAsync(int take, int skip, CancellationToken token = default)
        {
            return _setLocationService.GetSetLocationBackgroundOptionsAsync(take, skip, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long raceId, string filter = null, long? categoryId = null, long? taskId = null, int? characterCount = null, long? emotionId = null, long[] movementTypeIds = null, CancellationToken token = default)
        {
            return _bodyAnimationService.GetBodyAnimationListAsync(target, takeNext, takePrevious, raceId, filter, categoryId, taskId, characterCount, emotionId, movementTypeIds, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default)
        {
            return _bodyAnimationService.GetBodyAnimationListAsync(filterModel, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetMyBodyAnimationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default)
        {
            return _bodyAnimationService.GetMyBodyAnimationListAsync(target, takeNext, takePrevious, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long movementTypeId,
            int characterCount, long raceId, string filter = null, long? taskId = null, CancellationToken token = default)
        {
            return _bodyAnimationService.GetRecommendedBodyAnimationListAsync(target, takeNext, takePrevious,
                movementTypeId, characterCount, raceId, filter, taskId, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default)
        {
            return _bodyAnimationService.GetRecommendedBodyAnimationListAsync(filterModel, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationGroupAsync(long id, CancellationToken token = default)
        {
            return _bodyAnimationService.GetBodyAnimationGroupAsync(id, token);
        }

        public Task<Result<BodyAnimationInfo>> GetBodyAnimationAsync(long id, CancellationToken token = default)
        {
            return _bodyAnimationService.GetBodyAnimationAsync(id, token);
        }

        public Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationByIdsAsync(long[] ids, CancellationToken token = default)
        {
            return _bodyAnimationService.GetBodyAnimationByIdsAsync(ids, token);
        }

        public Task<ArrayResult<VfxInfo>> GetVfxListAsync(long? target, int takeNext, int takePrevious, long raceId, string filter = null, long? categoryId = null, long? taskId = null, bool? withAnimationOnly = false, CancellationToken token = default)
        {
            return _vfxService.GetVfxListAsync(target, takeNext, takePrevious, raceId, filter, categoryId, taskId, withAnimationOnly, token);
        }

        public Task<ArrayResult<VfxInfo>> GetVfxListAsync(VfxFilterModel filterModel, CancellationToken token = default)
        {
            return _vfxService.GetVfxListAsync(filterModel, token);
        }

        public Task<ArrayResult<VfxInfo>> GetMyVfxListAsync(long? target, int takeNext, int takePrevious, CancellationToken token)
        {
            return _vfxService.GetMyVfxListAsync(target, takeNext, takePrevious, token);
        }

        public Task<ArrayResult<CameraFilterInfo>> GetCameraFilterListAsync(long? target, int takeNext, int takePrevious,
            string filter = null, long? categoryId = null, long? taskId = null, CancellationToken token = default)
        {
            return _cameraFilterService.GetCameraFilterListAsync(target, takeNext, takePrevious, filter, categoryId, taskId, token);
        }

        public Task<ArrayResult<CameraFilterInfo>> GetMyCameraFilterListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default)
        {
            return _cameraFilterService.GetMyCameraFilterListAsync(target, takeNext, takePrevious, token);
        }

        public Task<Result<SongInfo>> GetSongAsync(long id, CancellationToken token)
        {
            return _songService.GetSongAsync(id, token);
        }

        public Task<ArrayResult<SongInfo>> GetSongsAsync(int take, int skip, string filter = null, long? genreId = null, long[] songIds = null, bool commercialOnly = false, long? emotionId = null, CancellationToken token = default)
        {
            return _songService.GetSongsAsync(take, skip, filter, genreId, songIds, commercialOnly, emotionId, token);
        }

        public Task<Result<UserSoundFullInfo>> GetUserSoundAsync(long id, CancellationToken token)
        {
            return _userSoundsService.GetUserSoundAsync(id, token);
        }

        public Task<ArrayResult<UserSoundFullInfo>> GetUserSoundsAsync(int take, int skip, CancellationToken token)
        {
            return _userSoundsService.GetUserSoundsAsync(take, skip, token);
        }
        
        public Task<ArrayResult<TrendingUserSound>> GetTrendingUserSoundsAsync(string searchQuery, int take, int skip, CancellationToken token)
        {
            return _userSoundsService.GetTrendingUserSoundsAsync(searchQuery, take, skip, token);
        }

        public Task<Result<UserSoundFullInfo>> UpdateUserSoundNameAsync(long id, string name, CancellationToken token)
        {
            return _userSoundsService.UpdateUserSoundNameAsync(id, name, token);
        }

        public Task<Result<UserSoundFullInfo>> CreateUserSoundAsync(CreateUserSoundModel model)
        {
            return _userSoundsService.CreateUserSoundAsync(model);
        }

        public Task<ArrayResult<FavouriteMusicInfo>> GetFavouriteSoundList(int take, int skip, bool commercialOnly, CancellationToken token)
        {
            return _favouriteMusicService.GetFavouriteSoundList(take, skip, commercialOnly, token);
        }

        public Task<Result<FavouriteMusicInfo>> AddSoundToFavouriteList(SoundType soundType, long id)
        {
            return _favouriteMusicService.AddSoundToFavouriteList(soundType, id);
        }

        public Task<Result> RemoveSoundFromFavouriteList(SoundType soundType, long id)
        {
            return _favouriteMusicService.RemoveSoundFromFavouriteList(soundType, id);
        }

        public Task<Result<OutfitFullInfo>> GetOutfitAsync(long id, CancellationToken token)
        {
            return _outfitService.GetOutfitAsync(id, token);
        }

        public Task<ArrayResult<OutfitShortInfo>> GetOutfitListAsync(int take, int skip, long genderId, SaveOutfitMethod outfitSaveType, CancellationToken token = default)
        {
            return _outfitService.GetOutfitListAsync(take, skip, outfitSaveType, genderId, token);
        }

        public Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryTypeId, long genderId, long? wardrobeCategoryId = null, long? wardrobeSubCategoryId = null, long? themeCollectionId = null, CancellationToken token = default)
        {
            return _wardrobeService.GetMyWardrobeList(target, takeNext, takePrevious, wardrobeCategoryTypeId, genderId:genderId, wardrobeCategoryId:wardrobeCategoryId, wardrobeSubCategoryId: wardrobeSubCategoryId, themeCollectionId, token);
        }

        public Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(MyWardrobeFilterModel filter, CancellationToken token = default)
        {
            return _wardrobeService.GetMyWardrobeList(filter, token);
        }

        public Task<ArrayResult<WardrobeFullInfo>> GetWardrobeList(long genderId, string[] wardrobeNames, CancellationToken token = default)
        {
            return _wardrobeService.GetWardrobeList(genderId, wardrobeNames, token);
        }

        public Task<ArrayResult<WardrobeCategoriesForGender>> GetWardrobeCategoriesPerGender(CancellationToken token = default)
        {
            return _wardrobeService.GetWardrobeCategoriesPerGender(token);
        }

        public Task<Result<WardrobeFullInfo>> GetWardrobe(long id, CancellationToken token = default)
        {
            return _wardrobeService.GetWardrobe(id, token);
        }

        public Task<Result<WardrobeFullInfo>> GetFitGenderWardrobe(long targetWardrobeId, long genderId, CancellationToken token = default)
        {
            return _wardrobeService.GetFitGenderWardrobe(targetWardrobeId, genderId, token);
        }

        public Task<Result<WardrobeFullInfo>> GetRewardWardrobe(IRewardModel reward, long genderId, CancellationToken token = default)
        {
            if (reward.Asset == null || reward.Asset.AssetType != AssetStoreAssetType.Wardrobe)
            {
                return Task.FromResult(Result<WardrobeFullInfo>.Error("Reward does not have a wardrobe"));
            }

            return GetFitGenderWardrobe(reward.Asset.Id, genderId, token);
        }

        public Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryId = null, long? wardrobeSubCategoryId = null, 
            long genderId = 0, AssetSorting sorting = AssetSorting.NewestFirst, AssetPriceFilter assetPriceFilter = AssetPriceFilter.None, long? taskId = null, long[] tags = null, long? themeCollectionId = null,  CancellationToken token = default)
        {
            return _wardrobeService.GetWardrobeList(target, takeNext, takePrevious, wardrobeCategoryId, wardrobeSubCategoryId, genderId, sorting, assetPriceFilter, taskId, tags, themeCollectionId, token);
        }
        
        public Task<Result<MyWardrobesListInfo>> GetMyWardrobeListInfo(long? wardrobeCategoryTypeId, long genderId, long? wardrobeCategoryId,
            long? wardrobeSubCategoryId, long? themeCollectionId, CancellationToken token = default)
        {
            return _wardrobeService.GetMyWardrobeListInfo(wardrobeCategoryTypeId, genderId, wardrobeCategoryId,
                wardrobeSubCategoryId, themeCollectionId, token);
        }

        public Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(WardrobeFilter filter, CancellationToken token = default)
        {
            return _wardrobeService.GetWardrobeList(filter, token);
        }
        
        public Task<Result<OutfitFullInfo>> SaveOutfitAsync(OutfitSaveModel saveModel)
        {
            return _outfitService.SaveOutfitAsync(saveModel);
        }

        public Task<Result> DeleteOutfit(long id)
        {
            return _outfitService.DeleteOutfit(id);
        }
        
        public Task<Result<RandomSetLocationSetup>> GetRandomSetLocationSetup(int characterCount, CancellationToken token)
        {
            return _randomAssetsService.GetRandomSetLocationSetup(characterCount, token);
        }

        public Task<ArrayResult<Emotion>> GetEmotionsAsync(int take, int skip, CancellationToken token = default)
        {
            return _emotionService.GetEmotionsAsync(take, skip, token);
        }

        public Task<Result<EmotionAssetsSetup>> GetEmotionAssetsSetupAsync(long emotionId, CancellationToken token = default)
        {
            return _emotionService.GetEmotionAssetsSetupAsync(emotionId, token);
        }

        public Task<ArrayResult<CountryInfo>> GetCountriesListAsync(CancellationToken token)
        {
            return _countryService.GetCountriesListAsync(token);
        }

        public async Task<Result<CountryInfo>> GetCountryInfoAsync(string isoCode, CancellationToken token = default)
        {
            if (_countryService == null)
            {
                SetupAuthBridge(Environment);
                
                var serviceUrlsResult = await _authService.GetServicesUrls(token);
                if (serviceUrlsResult.IsError)
                {
                    return Result<CountryInfo>.Error(serviceUrlsResult.ErrorMessage);
                }
                
                if (token.IsCancellationRequested) return Result<CountryInfo>.Cancelled();
                
                var url = Extensions.CombineUrls(serviceUrlsResult.Model.Client, "api");
                _countryService = new CountryService(url, _requestHelper, _serializer);
            }
            
            return await _countryService.GetCountryInfoAsync(isoCode, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetFriendsMainCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default)
        {
            return _characterService.GetFriendsMainCharacters(target, takeNext, takePrevious, universeId, filter, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetMyCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter, CancellationToken token)
        {
            return _characterService.GetMyCharacters(target, takeNext, takePrevious, universeId, filter, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetStarCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter, CancellationToken token)
        {
            return _characterService.GetStarCharacters(target, takeNext, takePrevious, universeId, filter, token);
        }

        public Task<ArrayResult<CharacterInfo>> GetStyleCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default)
        {
            return _characterService.GetStyleCharacters(target, takeNext, takePrevious, universeId, filter, token);
        }

        public Task<Result<CharacterFullInfo>> GetCharacter(long id, CancellationToken token)
        {
            return _characterService.GetCharacter(id, token);
        }

        public Task<ArrayResult<CharacterFullInfo>> GetCharacters(long[] ids, CancellationToken token = default)
        {
            return _characterService.GetCharacters(ids, token);
        }

        public Task<Result<CharacterFullInfo>> SaveCharacter(CharacterSaveModel character)
        {
            return _characterService.SaveCharacter(character);
        }

        public Task<Result> UpdateCharacterName(long characterId, string name)
        {
            return _characterService.UpdateCharacterName(characterId, name);
        }

        public Task<Result<CharacterFullInfo>> UpdateCharacterThumbnails(long characterId, params FileInfo[] thumbnails)
        {
            return _characterService.UpdateCharacterThumbnails(characterId, thumbnails);
        }

        public Task<Result> DeleteCharacter(long id)
        {
            return _characterService.DeleteCharacter(id);
        }
        
        public Task<ArrayResult<LevelShortInfo>> GetLevelDrafts(int take, int skip, CancellationToken token = default)
        {
            return _levelService.GetLevelDrafts(take, skip, token);
        }

        public Task<Result<LevelShortInfo>> GetLevelThumbnailInfo(long levelId, CancellationToken token = default)
        {
            return _levelService.GetLevelThumbnailInfo(levelId, token);
        }

        public Task<Result<LevelFullData>> GetLevel(long levelId, CancellationToken token = default)
        {
            return _levelService.GetLevel(levelId, token);
        }

        public Task<Result<LevelFullData>> GetLevelTemplateForVideoMessage(CancellationToken token = default)
        {
            return _levelService.GetLevelTemplateForVideoMessage(token);
        }

        public Task<Result<LevelFullData>> GetShuffledLevel(long levelId, CancellationToken token = default)
        {
            return _levelService.GetShuffledLevel(levelId, token);
        }

        public Task<Result<ShuffleMLResult>> GetShuffledLevelRemixAI(MlRemixRequest model, CancellationToken token = default)
        {
            return _levelService.GetShuffledLevelRemixAI(model, token);
        }

        public Task<Result<LevelShuffleResult>> GetShuffledLevel(LevelShuffleInput levelData, CancellationToken token = default)
        {
            return _levelService.GetShuffledLevel(levelData, token);
        }

        public Task<Result<LevelShuffleResult>> GetShuffledLevelAI(LevelShuffleInputAI levelData, CancellationToken token = default)
        {
            return _levelService.GetShuffledLevelAI(levelData, token);
        }

        public Task<Result<LevelFullData>> SaveLevel(LevelFullInfo level)
        {
            return _levelService.SaveLevel(level);
        }

        public Task<Result> UpdateLevelDescription(long levelId, string description)
        {
            return _levelService.UpdateLevelDescription(levelId, description);
        }

        public Task<UpdateFilesResult> UpdateEventThumbnails(Dictionary<long, List<FileInfo>> eventThumbnailData)
        {
            return _levelService.UpdateEventThumbnails(eventThumbnailData);
        }

        public Task<UpdateFilesResult> UpdateEventCameraAnimations(Dictionary<long, List<FileInfo>> eventCameraAnimations)
        {
            return _levelService.UpdateEventCameraAnimations(eventCameraAnimations);
        }

        public Task<Result> DeleteLevel(long levelId)
        {
            return _levelService.DeleteLevel(levelId);
        }

        public Task<ArrayResult<TaskInfo>> GetTasksAsync(long? targetTaskId, int takeNext, int takePrevious, string filter, TaskType? taskType, CancellationToken token = default)
        {
            return _taskService.GetTasksAsync(targetTaskId, takeNext, takePrevious, filter, taskType, token);
        }

        public Task<ArrayResult<TaskInfo>> GetJoinedVotingTasks(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default)
        {
            return _taskService.GetJoinedVotingTasks(targetTaskId, takeNext, takePrevious, token);
        }

        public Task<CountResult> GetJoinedVotingTasksCount(CancellationToken token)
        {
            return _taskService.GetJoinedVotingTasksCount(token);
        }

        public Task<NextTaskReleaseDateResult> GetNextTaskReleaseDate(CancellationToken token = default)
        {
            return _taskService.GetNextTaskReleaseDate(token);
        }

        public Task<ArrayResult<TaskInfo>> GetTrendingTasksAsync(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token)
        {
            return _taskService.GetTrendingTasksAsync(targetTaskId, takeNext, takePrevious, token);
        }

        public Task<ArrayResult<TaskInfo>> GetUserTasksAsync(long userGroupId, long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default)
        {
            return _taskService.GetUserTasksAsync(userGroupId, targetTaskId,takeNext, takePrevious, token);
        }

        public Task<ArrayResult<TaskInfo>> GetCurrentUserTasksAsync(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default)
        {
            return GetUserTasksAsync(Profile.GroupId, targetTaskId, takeNext, takePrevious, token);
        }

        public Task<Result<TaskFullInfo>> GetTaskFullInfoAsync(long taskId, CancellationToken token = default)
        {
            return _taskService.GetTaskFullInfoAsync(taskId, token);
        }

        public Task<Result<LevelFullData>> GetLevelForTaskAsync(long taskId, CancellationToken token)
        {
            return _taskService.GetLevelForTaskAsync(taskId, token);
        }

        public Task<Result<CurrentSeason>> GetCurrentSeason(CancellationToken token = default)
        {
            return _seasonService.GetCurrentSeason(token);
        }

        public Task<ClaimRewardResult> ClaimOnboardingReward(long onboardingRewardId)
        {
            return _rewardClaimingService.ClaimOnboardingReward(onboardingRewardId);
        }

        public Task<ClaimRewardResult> ClaimDailyQuestReward(long dailyQuestId)
        {
            return _rewardClaimingService.ClaimDailyQuestReward(dailyQuestId);
        }

        public Task<ClaimRewardResult> ClaimLevelReward(long levelRewardId)
        {
            return _rewardClaimingService.ClaimLevelReward(levelRewardId);
        }

        public Task<ClaimRewardResult> ClaimSeasonQuestReward(long seasonQuestId)
        {
            return _rewardClaimingService.ClaimSeasonQuestReward(seasonQuestId);
        }
        
        public Task<ClaimRewardResult> ClaimCreatorScoreReward(long rewardId)
        {
            return _rewardClaimingService.ClaimCreatorScoreReward(rewardId);
        }

        public Task<ClaimRewardResult> ClaimVideoRaterReward(long rewardId)
        {
            return _rewardClaimingService.ClaimVideoRaterReward(rewardId);
        }

        public Task<ClaimRewardResult> ClaimRatedVideoReward(long videoId)
        {
            return _rewardClaimingService.ClaimRatedVideoReward(videoId);
        }

        public Task<Result<ClaimPastRewardsResult>> ClaimPastRewards()
        {
            return _rewardClaimingService.ClaimPastRewards();
        }

        public Task<ClaimRewardResult> ClaimVotingBattleReward(long taskId)
        {
            return _rewardClaimingService.ClaimVotingBattleReward(taskId);
        }

        public Task<Result> ClaimRewardForInvitedUser()
        {
            return _rewardClaimingService.ClaimRewardForInvitedUser();
        }

        public Task<Result> ClaimRewardFromInvitedUser(long invitedUserGroupId)
        {
            return _rewardClaimingService.ClaimRewardFromInvitedUser(invitedUserGroupId);
        }
        
        public Task<ClaimRewardResult> ClaimTrophyHuntReward(long rewardId)
        {
            return _rewardClaimingService.ClaimTrophyHuntReward(rewardId);
        }

        public Task<Result> BuyPremium()
        {
            return _premiumPassService.BuyPremium();
        }

        public Task<Result<AvailableOffers>> GetProductOffers(CancellationToken token = default)
        {
            return _inAppPurchasesService.GetProductOffers(token);
        }

        public Task<InitPurchasingResult> InitPurchasingInAppProduct(string productOfferKey, string currency, decimal price)
        {
            return _inAppPurchasesService.InitPurchasingInAppProduct(productOfferKey, currency, price);
        }

        public Task<Result> CompletePurchasingInAppProduct(Guid pendingOrderId, string receipt)
        {
            return _inAppPurchasesService.CompletePurchasingInAppProduct(pendingOrderId, receipt, Platform);
        }

        public Task<Result> ExchangeHardCurrency(long exchangeProductId)
        {
            return _inAppPurchasesService.ExchangeHardCurrency(exchangeProductId, Platform);
        }

        public Task<ArrayResult<Deal>> GetDeals(CancellationToken token)
        {
            return _dealsService.GetDeals(token);
        }
        
        public Task<Result<AssetPurchaseResult>> PurchaseSeasonLevel(int level)
        {
            return _dealsService.PurchaseSeasonLevel(level);
        }

        public Task<Result<AssetPurchaseResult>> PurchaseAsset(long assetOfferId)
        {
            return _dealsService.PurchaseAsset(assetOfferId);
        }

        public Task<Result<EditorsSettings>> GetDefaultSettings(CancellationToken token = default)
        {
            return _editorSettingsService.GetDefaultSettings(token);
        }

        public Task<Result<EditorsSettings>> GetRemixSettings(CancellationToken token = default)
        {
            return _editorSettingsService.GetRemixSettings(token);
        }

        public Task<Result<EditorsSettings>> GetSettingForTask(long taskId, CancellationToken token = default)
        {
            return _editorSettingsService.GetSettingForTask(taskId, token);
        }

        public Task<ArrayResult<SocialActionFullInfo>> GetPersonalisedSocialActions(string treatmentGroup = null, IDictionary<string, string> headers = null, CancellationToken token = default)
        {
            return _socialActionsService.GetPersonalisedSocialActions(treatmentGroup, headers, token);
        }

        public Task<Result> DeleteSocialAction(Guid recommendationId, long actionId)
        {
            return _socialActionsService.DeleteAction(recommendationId, actionId);
        }

        public Task<Result> MarkActionAsComplete(Guid recommendationId, long actionId)
        {
            return _socialActionsService.MarkActionAsComplete(recommendationId, actionId);
        }

        public Task<Result<SoundsResult>> GetSounds(long[] songIds, long[] userSoundIds, long[] externalSongIds,
            CancellationToken token)
        {
            return _musicService.GetSounds(songIds, userSoundIds, externalSongIds, token);
        }

        public Task<Result<AgeConfirmationQuizResponse>> GetQuizStatus(CancellationToken token = default)
        {
            return _ageControlService.GetQuizStatus(token);
        }

        public Task<Result<AgeConfirmationQuizResponse>> GetAgeConfirmationQuestions(CancellationToken token = default)
        {
            return _ageControlService.GetAgeConfirmationQuestions(token);
        }

        public Task<Result<AgeConfirmationQuizResponse>> ConfirmAgeWithAnswers(AgeConfirmationAnswer[] answers)
        {
            return _ageControlService.ConfirmAgeWithAnswers(answers);
        }
    }
}