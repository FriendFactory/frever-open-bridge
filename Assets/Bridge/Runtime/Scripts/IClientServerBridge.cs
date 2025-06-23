using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer;
using Bridge.ClientServer.Assets.BodyAnimations;
using Bridge.ClientServer.Assets.Music;
using Bridge.ClientServer.Assets.SetLocations;
using Bridge.ClientServer.Assets.Vfxs;
using Bridge.ClientServer.Assets.Wardrobes;
using Bridge.ClientServer.InAppPurchases;
using Bridge.ClientServer.Localization;
using Bridge.ClientServer.Tasks;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.ClientServer.EditorsSetting;
using Bridge.Models.ClientServer.Gamification;
using Bridge.Models.ClientServer.Gamification.Reward;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Models.ClientServer.Level;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Level.Shuffle;
using Bridge.Models.ClientServer.Recommendations;
using Bridge.Models.ClientServer.SocialActions;
using Bridge.Models.ClientServer.StartPack;
using Bridge.Models.ClientServer.Tasks;
using Bridge.Models.Common.Files;
using Bridge.Results;

namespace Bridge
{
    public interface IClientServerBridge : ICharacterBridge, ILevelBridge, IWardrobeBridge, ITasksBridge, ISeasonBridge,
        IEditorSettingsBridge, IInAppPurchasesBridge, IInvitationBridge, IRecommendationsBridge, ISocialActionsBridge, IRewardsBridge, IMusicBridge, 
        ILocalizationBridge, IAgeControlBridge
    {
        Task<Result<SetLocationFullInfo>> GetSetLocationAsync(long id, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, long? setLocationCategoryId = null, string filter = null, long? taskId = null, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(SetLocationFilterModel filter, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetVideoMessageSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetMySetLocationListAsync(long? target, int takeNext, int takePrevious,
            CancellationToken token = default);
        Task<ArrayResult<SetLocationBackground>> GetSetLocationBackgroundListAsync(int take, int skip, CancellationToken token = default);
        Task<Result<BackgroundOptions>> GetSetLocationBackgroundOptionsAsync(int take, int skip, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long raceId, string filter = null, long? categoryId = null, long? taskId = null, int? characterCount = null, long? emotionId = null, long[] movementTypeIds = null, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetMyBodyAnimationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long movementTypeId, int characterCount, long raceId, string filter = null, 
            long? taskId = null, CancellationToken token = default);   
        Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default);
        Task<Result<BodyAnimationInfo>> GetBodyAnimationAsync(long id, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationByIdsAsync(long[] ids, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationGroupAsync(long id, CancellationToken token = default);

        Task<ArrayResult<VfxInfo>> GetVfxListAsync(long? target, int takeNext, int takePrevious, long raceId, string filter = null, long? categoryId = null, long? taskId = null, bool? withAnimationOnly = false, CancellationToken token = default);
        Task<ArrayResult<VfxInfo>> GetVfxListAsync(VfxFilterModel filterModel, CancellationToken token = default);
        Task<ArrayResult<VfxInfo>> GetMyVfxListAsync(long? target, int takeNext, int takePrevious,
            CancellationToken token = default);
        
        Task<ArrayResult<CameraFilterInfo>> GetCameraFilterListAsync(long? target, int takeNext, int takePrevious, string filter = null,
            long? categoryId = null, long? taskId = null, CancellationToken token = default);
        Task<ArrayResult<CameraFilterInfo>> GetMyCameraFilterListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default);
        Task<ArrayResult<CountryInfo>> GetCountriesListAsync(CancellationToken token = default);
        Task<Result<CountryInfo>> GetCountryInfoAsync(string isoCode, CancellationToken token = default);
        
        Task<Result<RandomSetLocationSetup>> GetRandomSetLocationSetup(int characterCount, CancellationToken token = default);
        Task<ArrayResult<Emotion>> GetEmotionsAsync(int take, int skip, CancellationToken token = default);
        Task<Result<EmotionAssetsSetup>> GetEmotionAssetsSetupAsync(long emotionId, CancellationToken token = default);
    }
    
    public interface ICharacterBridge
    {
        Task<ArrayResult<CharacterInfo>> GetFriendsMainCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterInfo>> GetMyCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterInfo>> GetStarCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<ArrayResult<CharacterInfo>> GetStyleCharacters(long? target, int takeNext, int takePrevious, long universeId, string filter = null, CancellationToken token = default);
        Task<Result<CharacterFullInfo>> GetCharacter(long id, CancellationToken token = default);
        Task<ArrayResult<CharacterFullInfo>> GetCharacters(long[] ids, CancellationToken token = default);
        Task<Result<CharacterFullInfo>> SaveCharacter(CharacterSaveModel character);
        Task<Result> UpdateCharacterName(long characterId, string name);
        Task<Result<CharacterFullInfo>> UpdateCharacterThumbnails(long characterId, params FileInfo[] thumbnails);
        Task<Result> DeleteCharacter(long id);
    }

    public interface ILevelBridge
    {
        Task<ArrayResult<LevelShortInfo>> GetLevelDrafts(int take, int skip, CancellationToken token = default);
        Task<Result<LevelShortInfo>> GetLevelThumbnailInfo(long levelId, CancellationToken token = default);
        Task<Result<LevelFullData>> GetLevel(long levelId, CancellationToken token = default);
        Task<Result<LevelFullData>> GetLevelTemplateForVideoMessage(CancellationToken token = default);
        Task<Result<LevelFullData>> GetShuffledLevel(long levelId, CancellationToken token = default);
        Task<Result<ShuffleMLResult>> GetShuffledLevelRemixAI(MlRemixRequest model, CancellationToken token = default);
        Task<Result<LevelShuffleResult>> GetShuffledLevel(LevelShuffleInput levelData, CancellationToken token = default);
        Task<Result<LevelShuffleResult>> GetShuffledLevelAI(LevelShuffleInputAI levelData, CancellationToken token = default);
        Task<Result<LevelFullData>> SaveLevel(LevelFullInfo level);
        Task<Result> UpdateLevelDescription(long levelId, string description);
        Task<UpdateFilesResult> UpdateEventThumbnails(Dictionary<long, List<FileInfo>> eventThumbnailData);
        Task<UpdateFilesResult> UpdateEventCameraAnimations(Dictionary<long, List<FileInfo>> eventCameraAnimations);
        Task<Result> DeleteLevel(long levelId);
    }

    public interface IWardrobeBridge
    {
        Task<ArrayResult<WardrobeCategoriesForGender>> GetWardrobeCategoriesPerGender(CancellationToken token = default);
        Task<Result<WardrobeFullInfo>> GetWardrobe(long id, CancellationToken token = default);
        Task<Result<WardrobeFullInfo>> GetFitGenderWardrobe(long targetWardrobeId, long genderId, CancellationToken token = default);
        Task<Result<WardrobeFullInfo>> GetRewardWardrobe(IRewardModel reward, long genderId, CancellationToken token = default);
        Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryTypeId, long genderId = 0, long? wardrobeCategoryId = null, long? wardrobeSubCategoryId = null, long? themeCollectionId = null, CancellationToken token = default);
        Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(MyWardrobeFilterModel filter, CancellationToken token = default);
        Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(long? target, int takeNext, int takePrevious,
            long? wardrobeCategoryId = null, long? wardrobeSubCategoryId = null, 
            long genderId = 0, AssetSorting sorting = AssetSorting.NewestFirst, AssetPriceFilter assetPriceFilter = AssetPriceFilter.None, 
            long? taskId = null, long[] tags = null, long? themeCollectionId = null, CancellationToken token = default);
        Task<Result<MyWardrobesListInfo>> GetMyWardrobeListInfo(long? wardrobeCategoryTypeId, long genderId, long? wardrobeCategoryId, long? wardrobeSubCategoryId, long? themeCollectionId = null, CancellationToken token = default);
        Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(WardrobeFilter filter, CancellationToken token = default);
        Task<Result<OutfitFullInfo>> GetOutfitAsync(long id, CancellationToken token);
        Task<ArrayResult<OutfitShortInfo>> GetOutfitListAsync(int take, int skip, long genderId, SaveOutfitMethod outfitSaveType = SaveOutfitMethod.Manual, CancellationToken token = default);
        Task<ArrayResult<WardrobeFullInfo>> GetWardrobeList(long genderId, string[] wardrobeNames, CancellationToken token = default);
        Task<Result<OutfitFullInfo>> SaveOutfitAsync(OutfitSaveModel saveModel);
        Task<Result> DeleteOutfit(long id);
    }
    
    public interface ITasksBridge
    {
        Task<ArrayResult<TaskInfo>> GetTasksAsync(long? targetTaskId, int takeNext, int takePrevious, string filter = null, TaskType? taskType = null, CancellationToken token = default);
        
        Task<ArrayResult<TaskInfo>> GetJoinedVotingTasks(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default);
        
        Task<CountResult> GetJoinedVotingTasksCount(CancellationToken token = default);
        
        Task<NextTaskReleaseDateResult> GetNextTaskReleaseDate(CancellationToken token = default);

        Task<ArrayResult<TaskInfo>> GetTrendingTasksAsync(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default);
        
        Task<ArrayResult<TaskInfo>> GetUserTasksAsync(long userGroupId, long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default);
        
        Task<ArrayResult<TaskInfo>> GetCurrentUserTasksAsync(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token = default);

        Task<Result<TaskFullInfo>> GetTaskFullInfoAsync(long taskId, CancellationToken token = default);

        Task<Result<LevelFullData>> GetLevelForTaskAsync(long taskId, CancellationToken token = default);
    }
    
    public interface ISeasonBridge
    {
        Task<Result<CurrentSeason>> GetCurrentSeason(CancellationToken token = default);
        Task<Result> BuyPremium();
    }

    public interface IRewardsBridge
    {
        Task<ClaimRewardResult> ClaimDailyQuestReward(long dailyQuestId);
        Task<ClaimRewardResult> ClaimLevelReward(long levelRewardId);
        Task<ClaimRewardResult> ClaimSeasonQuestReward(long seasonQuestId);
        Task<Result<ClaimPastRewardsResult>> ClaimPastRewards();
        Task<ClaimRewardResult> ClaimOnboardingReward(long onboardingRewardId);
        Task<ClaimRewardResult> ClaimTrophyHuntReward(long rewardId);
        Task<ClaimRewardResult> ClaimCreatorScoreReward(long rewardId);
        Task<ClaimRewardResult> ClaimVideoRaterReward(long rewardId);
        Task<ClaimRewardResult> ClaimRatedVideoReward(long videoId);
        Task<Result> ClaimRewardForInvitedUser();
        Task<Result> ClaimRewardFromInvitedUser(long invitedUserGroupId);
    }
    
    public interface IInAppPurchasesBridge
    {
        Task<Result<AvailableOffers>> GetProductOffers(CancellationToken token = default);
        Task<InitPurchasingResult> InitPurchasingInAppProduct(string productOfferKey, string currency = null, decimal price = 0);
        Task<Result> CompletePurchasingInAppProduct(Guid pendingOrderId, string receipt);
        Task<Result<AssetPurchaseResult>> PurchaseSeasonLevel(int level);
        Task<Result> ExchangeHardCurrency(long exchangeProductId);
        Task<ArrayResult<Deal>> GetDeals(CancellationToken token = default);
        Task<Result<AssetPurchaseResult>> PurchaseAsset(long assetOfferId);
    }
    
    public interface IEditorSettingsBridge
    {
        Task<Result<EditorsSettings>> GetDefaultSettings(CancellationToken token = default);
        
        Task<Result<EditorsSettings>> GetRemixSettings(CancellationToken token = default);
        
        Task<Result<EditorsSettings>> GetSettingForTask(long taskId, CancellationToken token = default);
    }
    
    public interface IInvitationBridge
    {
        Task<Result<InvitationCode>> GetInvitationCode(CancellationToken token = default);
        Task<Result<InviteeReward>> UseInvitationCode(Guid invitationGuid);
        Task<Result> SaveInvitationCode(string code);
        Task<Result<InviteeReward>> GetUnclaimedInviteeReward(CancellationToken token = default);
        Task<Result<string>> GetCreatorCode(CancellationToken token = default);
        Task<Result<StarCreator>> UseCreatorCode(string code);
        Task<Result> DeleteCreatorCode();
        Task<CountResult> GetCreatorAcceptedInvitationsCount(CancellationToken token = default);
    }

    public interface IRecommendationsBridge
    {
        Task<ArrayResult<FollowRecommendation>> GetFollowRecommendations(IDictionary<string, string> headers, CancellationToken token);
        Task<ArrayResult<FollowRecommendation>> GetFollowBackRecommendations(IDictionary<string, string> headers, CancellationToken token);
    }

    public interface ISocialActionsBridge
    {
        Task<ArrayResult<SocialActionFullInfo>> GetPersonalisedSocialActions(string treatmentGroup = null, IDictionary<string, string> headers = null, CancellationToken token = default);
        Task<Result> DeleteSocialAction(Guid recommendationId, long actionId);
        Task<Result> MarkActionAsComplete(Guid recommendationId, long actionId);
    }

    public interface IMusicBridge: IExternalMusicBridge, IFavouriteMusicService
    {
        Task<Result<SoundsResult>> GetSounds(long[] songIds, long[] userSoundIds, long[] externalSongIds,
            CancellationToken token);
        Task<Result<SongInfo>> GetSongAsync(long id, CancellationToken token);
        Task<ArrayResult<SongInfo>> GetSongsAsync(int take, int skip, string filter = null, long? genreId = null, long[] songIds = null, bool commercialOnly = false,
            long? emotionId = null, CancellationToken token = default);
        Task<Result<UserSoundFullInfo>> GetUserSoundAsync(long id, CancellationToken token);
        Task<ArrayResult<UserSoundFullInfo>> GetUserSoundsAsync(int take, int skip, CancellationToken token = default);
        Task<ArrayResult<TrendingUserSound>> GetTrendingUserSoundsAsync(string searchQuery, int take, int skip = 0, CancellationToken token = default);
        Task<Result<UserSoundFullInfo>> UpdateUserSoundNameAsync(long id, string name, CancellationToken token = default);
        Task<Result<UserSoundFullInfo>> CreateUserSoundAsync(CreateUserSoundModel model);
    }
    
    public interface ILocalizationBridge
    {
        bool HasCached(string isoCode, out string path);
        Task<LocalizationDataResult> GetLocalizationData(string isoCode, CancellationToken token = default);
    }

    public interface IAgeControlBridge
    {
        Task<Result<AgeConfirmationQuizResponse>> GetQuizStatus(CancellationToken token = default);
        Task<Result<AgeConfirmationQuizResponse>> GetAgeConfirmationQuestions(CancellationToken token = default);
        Task<Result<AgeConfirmationQuizResponse>> ConfirmAgeWithAnswers(AgeConfirmationAnswer[] answers);
    }
}