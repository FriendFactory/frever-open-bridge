using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace Tests.MainServer.DeserializationFileInfo
{
    public class DeserializationFileInfoTests 
    {
        [Test]
        public void DeserializeJson_ShouldReadFilesInfoProperly()
        {
            var json =
                "{\"id\":374,\"files\":[{\"uploadId\":null,\"version\":\"F.bysXbkScQWIIzQerw3EjAXz.A5jFnE\",\"extension\":5,\"file\":2},{\"uploadId\":null,\"version\":\"dOjdo6QIgbWmHWGiU7F1yYvcmVq_gwi1\",\"extension\":5,\"file\":1},{\"uploadId\":null,\"version\":\"UyqF0GUYk5hA3M2CuR7OXD0vZJsIGw5j\",\"extension\":5,\"file\":3},{\"uploadId\":null,\"version\":\"XDAISJG6WsI6Qrl4SeyQvcLbQxaicbr.\",\"extension\":5,\"file\":4}]}";

            var deserializedModel = JsonConvert.DeserializeObject<CharacterSpawnPosition>(json, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            Assert.True(deserializedModel.Files!=null);
            Assert.True(deserializedModel.Files.Count>0);
            Assert.True(deserializedModel.Files.TrueForAll(x => !string.IsNullOrEmpty(x.Version)));
        }
    }
}
