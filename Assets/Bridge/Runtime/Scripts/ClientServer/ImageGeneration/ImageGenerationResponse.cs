using Newtonsoft.Json;

namespace Bridge.ClientServer.ImageGeneration
{
    public sealed class StabilityCreateImageResponse: IOkResponse
    {
        public bool Ok { get; set; }
        public string ErrorMessage { get; set; }
        public string UploadId { get; set; }
        public string LocalFilePath { get; set; }
        
        [JsonProperty]
        internal string SignedFileUrl { get; set; }
    }
}