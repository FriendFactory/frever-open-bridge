using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.ClientServer.ImageGeneration;
using UnityEngine;
using TaskStatus = Bridge.ClientServer.ImageGeneration.TaskStatus;

namespace ApiTests.ImageGeneration
{
    internal sealed class KlingImageGenerationTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private Texture2D _humanPicture;
        [SerializeField] private Texture2D _clothesPicture;
        
        protected override async void RunTestAsync()
        {
            var scheduleRequestModel = new ScheduleTryOnOutfitRequest
            {
                HumanImage = ConvertToBase64(_humanPicture),
                ClothesImage = ConvertToBase64(_clothesPicture)
            };
            var scheduleTaskRes = await Bridge.ScheduleTryOnWardrobeTask(scheduleRequestModel);
            if (scheduleTaskRes.IsError)
            {
                Debug.LogError($"Failed to schedule Kling Image generation. Reason: {scheduleTaskRes.ErrorMessage}");
                return;
            }

            Debug.Log($"### Scheduled. Task ID: {scheduleTaskRes.Model.TaskId}");
            
            int requestsCount = 20;
            const float requestsIntervalSec = 5f;
            string imageUrl = null;
            do
            {
                var taskDetailsResp = await Bridge.GetImageGenerationTaskStatus(scheduleTaskRes.Model.TaskId, default);
                Debug.Log($"### Task status: {taskDetailsResp.Model.TaskStatus}");
                if (taskDetailsResp.IsSuccess && taskDetailsResp.Model.TaskStatus == TaskStatus.Succeed)
                {
                    imageUrl = taskDetailsResp.Model.ImageUrls.First();
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(requestsIntervalSec));
            } while (--requestsCount > 0);
            
            Debug.Log($"Image url: {imageUrl}");
        }
        
        public static string ConvertToBase64(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogError("Texture is null.");
                return null;
            }

            // Encode to PNG or JPG (you can choose EncodeToJPG for smaller size)
            byte[] imageBytes = texture.EncodeToPNG(); // or texture.EncodeToJPG(80); for JPG with quality 80

            // Convert to Base64 string
            return Convert.ToBase64String(imageBytes);
        }
    }
}