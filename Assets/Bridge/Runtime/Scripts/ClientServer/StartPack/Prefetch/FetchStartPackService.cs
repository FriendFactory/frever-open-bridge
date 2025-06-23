using System.Linq;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Models.AsseManager;
using Bridge.Modules.Serialization;

namespace Bridge.ClientServer.StartPack.Prefetch
{
    internal sealed class FetchStartPackService: StartPackServiceBase<PreFetchPack>
    {
        protected override string EndPointName => "pre-fetch";

        public FetchStartPackService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        protected override PreFetchPack ReadResponse(HTTPResponse response)
        {
            return Serializer.DeserializeProtobuf<PreFetchPack>(response.Data);
        }
    }
}