using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using BestHTTP.Addons.TLSSecurity;
using BestHTTP.Caching;
using BestHTTP.Logger;
using Bridge.AccountVerification;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Authorization.Configs;
using Bridge.Authorization.LocalStorage;
using Bridge.Authorization.LocalStorage.Storage;
using Bridge.Authorization.Models;
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
using Bridge.ClientServer.Battles;
using Bridge.ClientServer.Chat;
using Bridge.ClientServer.EditorSettings;
using Bridge.ClientServer.Emotions;
using Bridge.ClientServer.ImageGeneration;
using Bridge.ClientServer.InAppPurchases;
using Bridge.ClientServer.Invitation;
using Bridge.ClientServer.Level;
using Bridge.ClientServer.Localization;
using Bridge.ClientServer.Premium;
using Bridge.ClientServer.Recommendations;
using Bridge.ClientServer.Rewards;
using Bridge.ClientServer.SocialActions;
using Bridge.ClientServer.StartPack.Prefetch;
using Bridge.ClientServer.Tasks;
using Bridge.ClientServer.Template;
using Bridge.Constants;
using Bridge.EnvironmentCompatibility;
using Bridge.Models.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.NotificationServer;
using Bridge.Results;
using Bridge.Services._7Digital;
using Bridge.Services.AdTracking;
using Bridge.Services.Advertising;
using Bridge.Services.AssetService;
using Bridge.Services.AssetService.Caching;
using Bridge.Services.AssetService.Caching.AssetReaders;
using Bridge.Services.AssetService.Caching.Encryption;
using Bridge.Services.ContentModeration;
using Bridge.Services.CreatePage;
using Bridge.Services.Crews;
using Bridge.Services.SelfieAvatar;
using Bridge.Services.TranscodingService;
using Bridge.Services.UserProfile;
using Bridge.Settings;
using Bridge.VideoServer;
using UnityEngine;

[assembly: InternalsVisibleTo("Bridge.Editor")]

namespace Bridge
{
    public sealed partial class ServerBridge : IBridge
    {
        private readonly ICompatibilityService _compatibilityService;
        private readonly AuthServerConfigurations _authServerConfigurations;
        private readonly ProxyManager _proxyManager;
        private readonly IUserDataStorage _userDataStorage;
        private readonly ISerializer _serializer;
        private readonly IBridgeSettings _bridgeSettings;
        private readonly AssetReaderProvider _assetReaderProvider;
        private readonly EncryptionService _encryptionService;

        private IRequestHelper _requestHelper;
        private IAssetService _assetService;
        private IStorageFileCache _storageFileCache;
        private IAssetManagerServerBridge _assetManagerServerBridge;
        private IWardrobeService _wardrobeService;
        private IVideoServer _videoServer;
        private IVideoReportService _videoReportService;
        private IBattleService _battleService;
        private ISocialService _socialService;
        private INotificationService _notificationService;
        private IAuthService _authService;
        private AssetsCache _assetsCache;
        private SavingVersionFilesTrigger _savingVersionFilesTrigger;
        private IAdvertisingService _advertisingService;
        private SelfieToAvatar _selfieToAvatar;
        private IContentModerationService _contentModerationService;
        private readonly VersionProvider _versionProvider = new VersionProvider();
        private ITranscodingService _transcoder;
        private IMusicProviderService _musicProviderService;
        private IExternalTrackSearchService _externalTrackSearchService;
        private ICrewsService _crewsService;
        private IChatService _chatService;
        private IDealsService _dealsService;
        private IAdTrackingService _adTrackingService;
        private IImageGenerationService _imageGenerationService;
        private IAccountVerificationService _accountVerificationService;
        private readonly TempFileCache _tempFileCache;

        public ServerBridge()
        {
            _userDataStorage = new UserDataStorage();
            _userDataStorage.Load();
            _authServerConfigurations = new AuthServerConfigurations();
            
            UnityConstants.Init();
            _serializer = new Serializer();
            _bridgeSettings = GetBridgeSettings();
            _compatibilityService = new CompatibilityService(Version, ApiVersion, GetAllEnvironmentsAuthUrl());

            SetupBestHttp();
            SetupTlsCertificate();
            
            _proxyManager = new ProxyManager(_bridgeSettings);
            if (_proxyManager.ProxyEnabled)
            {
                _proxyManager.SetupGlobalForBestHttpRequests();
            }

            _encryptionService = new EncryptionService();
            _assetReaderProvider = new AssetReaderProvider(_encryptionService);
            _tempFileCache = new TempFileCache(ROOT_CACHE_FOLDER);
            
            SetupNewSession();
        }

        private void SetupBestHttp()
        {
            HTTPManager.Setup();
            HTTPManager.UserAgent = $"Bridge v.{Version}";
            PingFrequency = TimeSpan.FromSeconds(2);
            Timeout = TimeSpan.FromSeconds(6);
            
            ulong maxCacheSizeMb = 25;
            var cachingMaintainenceParams = new HTTPCacheMaintananceParams(TimeSpan.FromDays(1), maxCacheSizeMb * 1024 * 1024);
            HTTPCacheService.BeginMaintainence(cachingMaintainenceParams);
        }

        private Dictionary<FFEnvironment, string> GetAllEnvironmentsAuthUrl()
        {
            var envUrls = new Dictionary<FFEnvironment, string>();
            foreach (FFEnvironment environment in Enum.GetValues(typeof(FFEnvironment)))
            {
                var url = _authServerConfigurations.GetConfig(environment).IdentityServerURL;
                envUrls.Add(environment, url);
            }

            return envUrls;
        }
        
        public string Version => _versionProvider.BridgeVersion;
        private string ApiVersion => _versionProvider.ApiVersion;
        
        public TimeSpan PingFrequency
        {
            get => HTTPManager.HTTP2Settings.PingFrequency;
            set => HTTPManager.HTTP2Settings.PingFrequency = value;
        }
        public TimeSpan Timeout 
        { 
            get => HTTPManager.HTTP2Settings.Timeout;
            set => HTTPManager.HTTP2Settings.Timeout = value; 
        }

        public LogLevel LogLevel
        {
            get => (LogLevel)(int)HTTPManager.Logger.Level;
            set => HTTPManager.Logger.Level = (Loglevels)(int)value;
        }

        public Guid SessionId { get; private set; }

        public AuthToken Token => _authService.Token;
        public UserProfile Profile => _authService?.Profile;

        public bool RememberLastUser => _userDataStorage.HasSavedData;

        public long PublicGroupId => 1;
        
        public FFEnvironment? LastLoggedEnvironment
        {
            get
            {
                if (!_userDataStorage.HasSavedData)
                    return null;
                return _userDataStorage.UserData.FfEnvironment;
            }
        }
        public FFEnvironment Environment { get; private set; } = FFEnvironment.Production;
        public bool IsLoggedIn => _authService != null && _authService.IsLoggedIn;

        public void ChangeEnvironment(FFEnvironment next)
        {
            if (Environment == next)
            {
                Debug.LogWarning($"You are already connected to {next}");
                return;
            }

            if (IsLoggedIn)
               throw new Exception($"Please logout before changing server");

            Environment = next;

            _countryService = null;
        }

        public Task<ArrayResult<EnvironmentCompatibilityResult>> GetEnvironmentsCompatibilityData()
        {
            return _compatibilityService.GetEnvironmentsCompatibilityData();
        }

        public Task<EnvironmentCompatibilityResult> GetEnvironmentCompatibilityData(FFEnvironment environment)
        {
            return _compatibilityService.GetEnvironmentCompatibilityData(environment);
        }

        public Task<ArrayResult<LanguageInfo>> GetAvailableLanguages(CancellationToken token)
        {
            return _crewsService.GetAvailableLanguages(token);
        }

        private void OnUserAuthorized(bool savePassword)
        {
            if(savePassword) SaveUserData();
            InitializeCacheForSelectedEnvironment();

            var cacheSaveVersionsFileTriggerProvider= new SavingVersionFileTriggerProvider(_assetsCache);
            _savingVersionFilesTrigger = cacheSaveVersionsFileTriggerProvider.GetTrigger();
            _savingVersionFilesTrigger.Run();

            var assetDownloadRequestProvider = new AssetDownloadRequestProvider(_requestHelper);
            _storageFileCache = new StorageFileCache(ROOT_CACHE_FOLDER, Environment);
            _assetService = new AssetService(_authService.AssetSeverHost, _requestHelper, _authService, _assetReaderProvider,
                assetDownloadRequestProvider, _assetsCache, _storageFileCache, _platformsMap[_runtimePlatform], _serializer);
          
            var modelFilesUploader = new ModelsFileUploader(_assetService);
            _assetManagerServerBridge = new AssetManagerServerBridge(_authService.AssetManagerServerHost, _requestHelper, _assetsCache, _serializer, modelFilesUploader);
            _characterBakingService = new CharacterBakingService(_authService.AssetManagerServerHost, _requestHelper, _serializer, _assetService);
            _videoServer = new VideoServerBridge(_authService.VideoServerHost, _requestHelper, _serializer);
            _videoReportService = new VideoReportService(_authService.VideoServerHost, _requestHelper);
            _battleService = new BattleService(_authService.VideoServerHost, _requestHelper, _serializer);
            _socialService = new SocialService(_authService.ClientServerHost, _requestHelper, _serializer);
            _notificationService = new NotificationService(_authService.NotificationServerHost,_requestHelper, _serializer);
            _selfieToAvatar = new SelfieToAvatar(_requestHelper, Environment);
            _advertisingService = new AdvertisingService(_authService.ClientServerHost, _requestHelper, _serializer);
            _musicProviderService = new MusicProviderService(_authService.ClientServerHost, _requestHelper, _serializer);
            _externalTrackSearchService = new ExternalTrackSearchService(_authService.ClientServerHost, _requestHelper, _serializer);
            _crewsService = new CrewsService(_authService.ClientServerHost, _requestHelper, _serializer);
            _dealsService = new DealsService(_authService.ClientServerHost, _requestHelper, _serializer);
            _adTrackingService = new AdTrackingService(_authService.ClientServerHost, _requestHelper, _serializer);
            _accountVerificationService = new AccountVerificationService(_authService.AuthUrl, _requestHelper, _serializer);
            _createPageService = new CreatePageService(_authService.VideoServerHost, _requestHelper, _serializer);
            
            const string chatRootEndPoint = "chat";
            var postMessageService = new PostMessageService(_authService.ChatServerHost, _requestHelper, _serializer, modelFilesUploader, _assetsCache, chatRootEndPoint);
            _chatService = new ChatService(_authService.ChatServerHost, _requestHelper, _serializer, postMessageService, chatRootEndPoint);

            InitializeClientServerServices(modelFilesUploader);
        }

        private void InitializeClientServerServices(ModelsFileUploader fileUploader)
        {
            var host = _authService.ClientServerHost;
            _startPackService = new StartPackService(_requestHelper, _serializer, host);
            _levelService = new LevelService(host, _requestHelper, _serializer, fileUploader, _assetsCache);
            _wardrobeService = new WardrobeService(host, _requestHelper, _serializer);
            _setLocationService = new SetLocationService(host, _requestHelper, _serializer);
            _templateService = new TemplateService(host, _requestHelper, _serializer);
            _bodyAnimationService = new BodyAnimationService(host, _requestHelper, _serializer);
            _vfxService = new VfxService(host, _requestHelper, _serializer);
            _cameraFilterService = new CameraFilterService(host, _requestHelper, _serializer);
            _songService = new SongService(host, _requestHelper, _serializer);
            _userSoundsService = new UserSoundsService(host, _requestHelper, _serializer, fileUploader, _assetsCache);
            _characterService = new CharacterService(host, _requestHelper, _serializer, fileUploader, _assetsCache);
            _outfitService = new OutfitService(host, _requestHelper, _serializer, fileUploader, _assetsCache);
            _countryService = new CountryService(host, _requestHelper, _serializer);
            _taskService = new TaskService(host, _requestHelper, _serializer);
            var seasonService =  new SeasonService(host, _requestHelper, _serializer);
            _seasonService = seasonService;
            _rewardClaimingService = seasonService;
            _editorSettingsService = new EditorSettingsService(host, _requestHelper, _serializer);
            _inAppPurchasesService = new InAppPurchasesService(host, _requestHelper, _serializer);
            _premiumPassService = new PremiumPassService(host, _requestHelper, _serializer);
            _contentModerationService = new ContentModerationService(host, _serializer, _requestHelper);
            _randomAssetsService = new RandomAssetsService(host, _requestHelper, _serializer);
            _invitationService = new InvitationService(host, _requestHelper, _serializer);
            _recommendationsService = new RecommendationsService(host, _requestHelper, _serializer);
            _socialActionsService = new SocialActionsService(host, _requestHelper, _serializer);
            _transcoder = new TranscodingService(_authService.VideoServerHost, _assetService, _requestHelper, _contentModerationService, _serializer);
            _emotionService = new EmotionService(_authService.ClientServerHost, _requestHelper, _serializer);
            _musicService = new MusicService(host, _requestHelper, _serializer);
            _favouriteMusicService = new FavouriteMusicService(host, _requestHelper, _serializer);
            _ageControlService = new AgeControlService(host, _requestHelper, _serializer);
            var klingGenerationService = new KlingImageGenerationService("https://api.klingai.com", host, _requestHelper, _serializer);
            _imageGenerationService = new ImageGenerationService(host, _authService.VideoServerHost, _requestHelper, _serializer, _tempFileCache, klingGenerationService, _assetService);

            SetupLocalizationService();
        }

        partial void InitializeCacheForSelectedEnvironment();

        private void SaveUserData()
        {
            var userData = new UserData
            {
                FfEnvironment = Environment,
                Token = _authService.Token
            };
            _userDataStorage.Save(userData);
        }

        private static IBridgeSettings GetBridgeSettings()
        {
            var customSettings = Resources.Load<BridgeSettings>(nameof(BridgeSettings));
            return customSettings != null ? customSettings : new DefaultSettings();
        }
        
        private void SetupTlsCertificate()
        {
            if (_bridgeSettings.TlsSecurity)
            {
                TLSSecurity.Setup();
            }
        }
        
        private void SetupNewSession()
        {
            SessionId = Guid.NewGuid();
            _requestHelper = new RequestHelper(_proxyManager, SessionId.ToString(), _bridgeSettings);
        }
    }
}