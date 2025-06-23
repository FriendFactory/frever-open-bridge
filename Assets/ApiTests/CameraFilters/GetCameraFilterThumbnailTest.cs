using Bridge.Models.AsseManager;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.CameraFilters
{
    public class GetCameraFilterThumbnailTest: EntityApiTest<CameraFilter>
    {
        public long Id;
        protected override async void RunTestAsync()
        {
            var cameraFilterModelReq = await Bridge.GetAsync<CameraFilter>(Id);
            var fileReq = await Bridge.GetThumbnailAsync(cameraFilterModelReq.ResultObject, Resolution._128x128);
            if (fileReq.IsSuccess)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError(fileReq.ErrorMessage);
            }
        }
    }
}