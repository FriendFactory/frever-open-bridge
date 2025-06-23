using BestHTTP;
using Bridge.Modules.Serialization;

namespace Bridge.AssetManagerServer.ResponseReaders
{
    internal sealed class JsonResponseReader : ResponseReader
    {
        private const string JSON_CONTENT_TYPE = "application/json";
        
        public JsonResponseReader(ISerializer serializer) : base(serializer)
        {
        }
        
        public override bool CanRead(HTTPResponse resp)
        {
            return GetContentType(resp).StartsWith(JSON_CONTENT_TYPE);
        }
        
        public override T ReadObject<T>(HTTPResponse resp)
        {
            return Serializer.DeserializeJson<T>(resp.DataAsText);
        }

        public override T[] ReadArray<T>(HTTPResponse resp)
        {
            return Serializer.DeserializeJson<T[]>(resp.DataAsText);
        }
    }
}
