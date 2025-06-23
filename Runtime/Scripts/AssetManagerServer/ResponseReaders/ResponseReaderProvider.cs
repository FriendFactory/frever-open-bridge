using System.Linq;
using BestHTTP;
using Bridge.Modules.Serialization;

namespace Bridge.AssetManagerServer.ResponseReaders
{
    internal sealed class ResponseReaderProvider
    {
        private readonly ResponseReader[] _readers;

        public ResponseReaderProvider(ISerializer serializer)
        {
            _readers = new ResponseReader[]
            {
                new JsonResponseReader(serializer), 
                new ProtobufResponseReader(serializer)
            };
        }

        public ResponseReader GetResponseReader(HTTPResponse message)
        {
            return _readers.First(x => x.CanRead(message));
        }
    }
}