using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Constants;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.CameraFilters
{
    internal interface ICameraFilterService
    {
        Task<ArrayResult<CameraFilterInfo>> GetCameraFilterListAsync(long? target, int takeNext, int takePrevious, string filter = null,
            long? categoryId = null, long? taskId = null, CancellationToken token = default);
        Task<ArrayResult<CameraFilterInfo>> GetMyCameraFilterListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default);
    }

    internal sealed class CameraFilterService : AssetServiceBase, ICameraFilterService
    {
        public CameraFilterService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<CameraFilterInfo>> GetCameraFilterListAsync(long? target, int takeNext, int takePrevious, string filter = null, long? categoryId = null,
            long? taskId = null, CancellationToken token = default)
        {
            try
            {
                return await CameraFilterListAsyncInternal(target, takeNext, takePrevious, filter, categoryId, taskId, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<CameraFilterInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<CameraFilterInfo>> GetMyCameraFilterListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default)
        {
            try
            {
                return await GetMyCameraFilterListInternalAsync(target, takeNext, takePrevious, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<CameraFilterInfo>.Cancelled();
            }
        }

        private Task<ArrayResult<CameraFilterInfo>> CameraFilterListAsyncInternal(long? target, int takeNext, int takePrevious, string filter, long? categoryId, long? taskId, CancellationToken token)
        {
            var url = BuildUrl("CameraFilter");
            var body = new
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious,
                Name = filter,
                CameraFilterCategoryId = categoryId,
                TaskId = taskId,
                UnityConstants.UnityVersion
            };
            return SendRequestForListModels<CameraFilterInfo>(url, token, body);
        }
        
        private Task<ArrayResult<CameraFilterInfo>> GetMyCameraFilterListInternalAsync(long? target, int takeNext, int takePrevious, CancellationToken token)
        {
            var url = BuildUrl("CameraFilter/My");
            var body = new
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious
            };
            return SendRequestForListModels<CameraFilterInfo>(url, token, body);
        }
    }
}