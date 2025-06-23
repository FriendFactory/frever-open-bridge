using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Constants;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Vfxs
{
    internal interface IVfxService
    {
        Task<ArrayResult<VfxInfo>> GetVfxListAsync(long? target, int takeNext, int takePrevious, long raceId,
            string filter = null, long? categoryId = null, long? taskId = null, bool? withAnimationOnly = false,
            CancellationToken token = default);
        Task<ArrayResult<VfxInfo>> GetVfxListAsync(VfxFilterModel filterModel, CancellationToken token = default);

        Task<ArrayResult<VfxInfo>> GetMyVfxListAsync(long? target, int takeNext, int takePrevious,
            CancellationToken token);
    }

    internal sealed class VfxService: AssetServiceBase, IVfxService
    {
        public VfxService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<VfxInfo>> GetVfxListAsync(long? target, int takeNext, int takePrevious,
            long raceId, string filter = null, long? categoryId = null,
            long? taskId = null, bool? withAnimationOnly = false, CancellationToken token = default)
        {
            try
            {
                var body = new VfxFilterModel
                {
                    Target = target,
                    TakeNext = takeNext,
                    TakePrevious = takePrevious,
                    Name = filter,
                    VfxCategoryId = categoryId,
                    TaskId = taskId,
                    UnityVersion = UnityConstants.UnityVersion,
                    RaceId = raceId,
                    WithAnimationOnly = withAnimationOnly,
                };
                return await GetVfxListInternalAsync(body, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<VfxInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<VfxInfo>> GetVfxListAsync(VfxFilterModel filterModel, CancellationToken token = default)
        {
            try
            {
                return await GetVfxListInternalAsync(filterModel, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<VfxInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<VfxInfo>> GetMyVfxListAsync(long? target, int takeNext, int takePrevious, CancellationToken token)
        {
            try
            {
                return await GetMyVfxListInternalAsync(target, takeNext, takePrevious, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<VfxInfo>.Cancelled();
            }
        }

        private Task<ArrayResult<VfxInfo>> GetVfxListInternalAsync(VfxFilterModel filterModel, CancellationToken token)
        {
            var url = BuildUrl("Vfx");
            return SendRequestForListModels<VfxInfo>(url, token, filterModel);
        }
        
        private Task<ArrayResult<VfxInfo>> GetMyVfxListInternalAsync(long? target, int takeNext, int takePrevious, CancellationToken token)
        {
            var url = BuildUrl("Vfx/my");
            var body = new
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious
            };
            return SendRequestForListModels<VfxInfo>(url, token, body);
        }
    }
}