using BestHTTP;
using Bridge.Authorization;
using Bridge.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.StartPack.UserAssets;
using Bridge.Modules.Serialization;

namespace Bridge.ClientServer.StartPack.UserAssets
{
    internal sealed class UserAssetsStartPackService: StartPackServiceBase<DefaultUserAssets>
    {
        protected override string EndPointName => "default-user-assets";

        public UserAssetsStartPackService(string host, IRequestHelper requestHelper, ISerializer serializer) 
            : base(host, requestHelper, serializer)
        {
        }

        protected override DefaultUserAssets ReadResponse(HTTPResponse response)
        {
            return Serializer.DeserializeProtobuf<DefaultUserAssets>(response.Data);
        }
    }
}
