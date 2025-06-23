using Bridge.Authorization;
using Bridge.Modules.Serialization;

namespace Bridge.ClientServer
{
    internal abstract class AssetServiceBase: ServiceBase
    {
        protected const string RootEndPoint = "assets";
        
        protected AssetServiceBase(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
            
        }
        
        protected string BuildUrl(string assetTypeEndPoint)
        {
            return ConcatUrl(Host, $"{RootEndPoint}/{assetTypeEndPoint}");
        }
    }
}