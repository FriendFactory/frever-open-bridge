using Bridge.ExternalPackages.Protobuf;
using Newtonsoft.Json;

namespace Bridge.Models.Common.Files
{
    public partial class FileInfo
    {
        [JsonProperty("File")]
        public FileType FileType { get; set; }
        public Resolution? Resolution { get; set; }
        public FileSource Source { get; set; } = new FileSource();
        [JsonProperty, ProtoInclude]
        public string Version { get; internal set; }
        [JsonProperty, ProtoInclude]
        public Platform? Platform { get; set; }
        
        [JsonProperty, ProtoInclude, ProtoNewField(1)] 
        public string UnityVersion { get; set; }
        [JsonProperty, ProtoInclude, ProtoNewField(2)]  
        public string AssetManagerVersion { get; set; }
        [JsonProperty, ProtoInclude, ProtoNewField(3)]
        public string[] Tags { get; set; }
        [JsonProperty, ProtoInclude] public FileExtension Extension { get; set; }

        internal FileInfo()
        {
            //required for json deserialization
        }
    }
}
