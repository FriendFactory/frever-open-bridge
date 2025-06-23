using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.ClientServer.StartPack.Metadata;
using Bridge.ClientServer.StartPack.UserAssets;
using Bridge.Models.ClientServer.StartPack.Metadata;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Modules.Serialization;

namespace Bridge.ClientServer.StartPack.Prefetch
{
    internal interface IStartPackService
    {
        Task<StartPackResult<MetadataStartPack>> GetMetadataStartPackAsync(CancellationToken token = default);
        Task<StartPackResult<DefaultUserAssets>> GetUserStartupAssetsDataAsync(IDictionary<string, string> headers, CancellationToken token = default);
        Task<StartPackResult<PreFetchPack>> GetPreFetchStartPackAsync(CancellationToken token);
    }

    internal sealed class StartPackService: IStartPackService
    {
        private readonly FetchStartPackService _fetchStartPackService;
        private readonly StartPackMetadataService _startPackMetadataService;
        private readonly UserAssetsStartPackService _userAssetsStartPackService;

        public StartPackService(IRequestHelper requestHelper, ISerializer serializer, string host)
        {
            _fetchStartPackService = new FetchStartPackService(host, requestHelper, serializer);
            _startPackMetadataService = new StartPackMetadataService(host, requestHelper, serializer);
            _userAssetsStartPackService = new UserAssetsStartPackService(host, requestHelper, serializer);
        }

        public Task<StartPackResult<MetadataStartPack>> GetMetadataStartPackAsync(CancellationToken token = default)
        {
            return _startPackMetadataService.GetStartPackAsync(token:token);
        }

        public Task<StartPackResult<DefaultUserAssets>> GetUserStartupAssetsDataAsync(IDictionary<string, string> headers, CancellationToken token = default)
        {
            return _userAssetsStartPackService.GetStartPackAsync(headers, token);
        }

        public Task<StartPackResult<PreFetchPack>> GetPreFetchStartPackAsync(CancellationToken token)
        {
            return _fetchStartPackService.GetStartPackAsync(token:token);
        }
    }
}