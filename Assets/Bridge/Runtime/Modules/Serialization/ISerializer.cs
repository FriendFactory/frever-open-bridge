using System.IO;
using Newtonsoft.Json;

namespace Bridge.Modules.Serialization
{
    public interface ISerializer
    {
        string SerializeToJson(object obj, JsonSerializerSettings settings = null);
        T DeserializeJson<T>(string json, JsonSerializerSettings settings = null);

        byte[] SerializeProtobuf(object obj);
        T DeserializeProtobuf<T>(byte[] bytes);
        
        T DeserializeProtobuf<T>(Stream stream);
    }
}
