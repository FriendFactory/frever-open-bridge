using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using UnityEngine;


namespace Bridge.ClientServer.ImageGeneration
{
    internal interface IKlingImageGenerationService
    {
        int MaxImageSizeKb { get; }
        Vector2Int MinImageResolution { get; }
      
        Task<Result<ScheduleTryOnOutfitResponse>> ScheduleTryOnWardrobeTask(ScheduleTryOnOutfitRequest requestModel);
        Task<Result<TaskStatusResponse>> GetImageGenerationTaskStatus(string taskId, CancellationToken token = default);
    }
    
    public sealed class ScheduleTryOnOutfitRequest
    {
        /// <summary>
        /// Base64-encoded string
        /// </summary>
        public string HumanImage { get; set; }
        
        /// <summary>
        /// Base64-encoded string
        /// </summary>
        public string ClothesImage { get; set; }
    }
    
    public sealed class ScheduleTryOnOutfitResponse
    {
        public string TaskId { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }

    public sealed class TaskStatusResponse
    {
        public string TaskId { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string[] ImageUrls { get; set; }
    }
}