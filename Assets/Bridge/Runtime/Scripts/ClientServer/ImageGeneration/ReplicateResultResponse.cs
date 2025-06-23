using Newtonsoft.Json;

namespace Bridge.ClientServer.ImageGeneration
{
    public sealed class ReplicateResultResponse: IOkResponse
    {
        public bool IsReady { get; set; }

        public bool Ok { get; set; }

        public string ErrorMessage { get; set; }

        public string UploadId { get; set; }
        
        public string LocalFilePath { get; set; }

        [JsonProperty]
        internal string SignedFileUrl { get; set; }
    }
    
    internal sealed class ReplicateProgressResponse
    {
        public string PredictionId { get; set; }

        public bool Ok { get; set; }

        public string ErrorMessage { get; set; }
    }
}