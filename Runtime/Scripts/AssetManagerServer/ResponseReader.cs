using System;
using System.Linq;
using BestHTTP;
using Bridge.Modules.Serialization;

namespace Bridge.AssetManagerServer
{
    internal abstract class ResponseReader
    {
        protected readonly ISerializer Serializer;

        protected ResponseReader(ISerializer serializer)
        {
            Serializer = serializer;
        }
        
        public abstract bool CanRead(HTTPResponse resp);
        
        protected string GetContentType(HTTPResponse resp)
        {
            var contentTypeHeader = resp.Headers.First(x => x.Key.Equals("content-type", StringComparison.InvariantCultureIgnoreCase));
            var contentType = contentTypeHeader.Value.First();
            return contentType;
        }
        
        public abstract T ReadObject<T>(HTTPResponse resp);
        
        public abstract T[] ReadArray<T>(HTTPResponse resp);
    }
}