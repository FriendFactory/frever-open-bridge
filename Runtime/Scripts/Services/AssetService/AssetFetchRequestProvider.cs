using Bridge.Authorization;
using Bridge.Services.AssetService.Caching;

namespace Bridge.Services.AssetService
{
    internal sealed class AssetFetchRequestProvider
    {
        private readonly AssetsCache _assetsCache;
        private readonly IRequestHelper _requestHelper;

        public AssetFetchRequestProvider(AssetsCache assetsCache, IRequestHelper requestHelper)
        {
            _assetsCache = assetsCache;
            _requestHelper = requestHelper;
        }

        public FetchAssetRequest GetFetchAssetRequest()
        {
            return new FetchAssetRequest(_assetsCache, _requestHelper);
        }
    }
}