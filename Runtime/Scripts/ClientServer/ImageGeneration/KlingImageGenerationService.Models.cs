using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bridge.ClientServer.ImageGeneration
{
    internal sealed partial class KlingImageGenerationService
    {
         private sealed class TokenResponse
        {
            public string Token { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
        
        private sealed class TryOnRequestBody
        {
            [JsonProperty("human_image")]
            public string HumanImage { get; set; }
            
            [CanBeNull] 
            [JsonProperty("cloth_image")]
            public string ClothImage { get; set; }
            
            [CanBeNull]
            [JsonProperty("model_name")]
            public string ModelName { get; set; }
        }
        
        private sealed class TryOnResponse
        {
            [JsonProperty("code")]
            public int Code { get; set; }
    
            [JsonProperty("message")]
            public string Message { get; set; }
    
            [JsonProperty("request_id")]
            public string RequestId { get; set; }
    
            [JsonProperty("data")]
            public TaskData Data { get; set; }
        }

        private sealed class TaskData
        {
            [JsonProperty("task_id")]
            public string TaskId { get; set; }
    
            [JsonProperty("task_status")]
            [JsonConverter(typeof(StringEnumConverter))]
            public TaskStatus TaskStatus { get; set; }
    
            [JsonProperty("created_at")]
            public long CreatedAt { get; set; }
    
            [JsonProperty("updated_at")]
            public long UpdatedAt { get; set; }
            
            [JsonProperty("task_result")]
            public TaskResult TaskResult { get; set; }
    
            public DateTime CreatedAtDateTime => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt).UtcDateTime;
    
            public DateTime UpdatedAtDateTime => DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt).UtcDateTime;
        }
        
        private sealed class TaskResult
        {
            [JsonProperty("images")]
            public List<ImageData> Images { get; set; }
        }

        private sealed class ImageData
        {
            [JsonProperty("index")]
            public int Index { get; set; }
    
            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }
    
    public enum TaskStatus
    {
        Submitted,
        Processing,
        Succeed,
        Failed
    }
}