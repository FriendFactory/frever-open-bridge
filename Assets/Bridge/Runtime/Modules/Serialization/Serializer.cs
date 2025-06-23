using System.IO;
using Bridge.ExternalPackages.Protobuf;
using Newtonsoft.Json;

namespace Bridge.Modules.Serialization
{
    public sealed class Serializer: ISerializer
    {
        public string SerializeToJson(object obj, JsonSerializerSettings settings = null)
        {
            return settings!=null 
                ? JsonConvert.SerializeObject(obj, settings) 
                : JsonConvert.SerializeObject(obj);
        }

        public T DeserializeJson<T>(string json, JsonSerializerSettings settings = null)
        {
            return settings!=null 
                ? JsonConvert.DeserializeObject<T>(json, settings) 
                : JsonConvert.DeserializeObject<T>(json);
        }

        public byte[] SerializeProtobuf(object obj)
        {
            return ProtobufConvert.SerializeObject(obj);
        }

        public T DeserializeProtobuf<T>(byte[] bytes)
        {
            return ProtobufConvert.DeserializeObject<T>(bytes);
        }

        public T DeserializeProtobuf<T>(Stream stream)
        {
            return ProtobufConvert.DeserializeObject<T>(stream);
        }
    }
}