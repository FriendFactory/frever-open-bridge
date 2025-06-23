using BestHTTP;
using Bridge.Authorization;
using Bridge.ClientServer.StartPack.Prefetch;
using Bridge.Models.ClientServer.StartPack.Metadata;
using Bridge.Modules.Serialization;

namespace Bridge.ClientServer.StartPack.Metadata
{
    internal sealed class StartPackMetadataService: StartPackServiceBase<MetadataStartPack>
    {
        protected override string EndPointName => "metadata";
        
        public StartPackMetadataService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }
        
        protected override MetadataStartPack ReadResponse(HTTPResponse response)
        {
            return Serializer.DeserializeProtobuf<MetadataStartPack>(response.Data);
        }
    }
}