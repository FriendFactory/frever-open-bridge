using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer.Chat;
using Bridge.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.StartPack.Metadata;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Results;
using Bridge.Services.AdTracking;
using Bridge.Services.ContentModeration;
using Bridge.Services.Crews;

[assembly: InternalsVisibleTo("Tests")]

namespace Bridge
{
    public interface IBridge : IAuthBridge, IStorageBridge, IBridgeCache, IVideoBridge, ISocialBridge,
        INotificationsBridge, IEntitiesBridge, IS2ABridge, IConverterBridge, IAdsBridge, IEnvironmentControl,
        IStartPackBridge, IBridgeAutomationHelper, IClientServerBridge, IVotingBattlesBridge, IContentModerationBridge,
        IEncryptionBridge, ICrewsService, IChatService, IAdTrackingService, ICharacterBakingBridge, IAccountVerificationBridge, 
        ICreatePageBridge, IImageGenerationBridge
    {
        string Version { get; }
        
        TimeSpan PingFrequency { get; set; }
        TimeSpan Timeout { get; set; }
        
        LogLevel LogLevel { get; set; }
        Guid SessionId { get; }
    }

    public interface IStartPackBridge
    {
        Task<StartPackResult<MetadataStartPack>> GetMetadataStartPackAsync(CancellationToken token = default);
        Task<StartPackResult<DefaultUserAssets>> GetUserStartupAssetsDataAsync(IDictionary<string, string> headers, CancellationToken token = default);
        Task<Result> FetchStartPackAssets(int maxConcurrentRequests, Action<float> progressCallback = null, CancellationToken token = default);
    }
}