using BestHTTP;
using Bridge.Modules.Serialization;

namespace Bridge.AssetManagerServer.ResponseReaders
{
    internal sealed class ProtobufResponseReader : ResponseReader
    {
        private const string PROTO_CONTENT_TYPE = "application/vnd.google.protobuf";
        
        public ProtobufResponseReader(ISerializer serializer) : base(serializer)
        {
        }

        public override bool CanRead(HTTPResponse resp)
        {
            return GetContentType(resp) == PROTO_CONTENT_TYPE;
        }

        public override T ReadObject<T>(HTTPResponse resp)
        {
            return Serializer.DeserializeProtobuf<T>(resp.Data);
        }

        public override T[] ReadArray<T>(HTTPResponse resp)
        {
            return Serializer.DeserializeProtobuf<T[]>(resp.Data);
        }
    }
}
