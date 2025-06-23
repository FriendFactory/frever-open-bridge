using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Constants;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.SetLocations
{
    internal interface ISetLocationService
    {
        Task<Result<SetLocationFullInfo>> GetSetLocationAsync(long id, CancellationToken token);
        Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, long? setLocationCategoryId = null, string filter = null, long? taskId = null, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(SetLocationFilterModel filter, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetMySetLocationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default);
        Task<ArrayResult<SetLocationFullInfo>> GetVideoMessageSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, CancellationToken token);
        Task<ArrayResult<SetLocationBackground>> GetSetLocationBackgroundListAsync(int take, int skip, CancellationToken token);
        Task<Result<BackgroundOptions>> GetSetLocationBackgroundOptionsAsync(int take, int skip, CancellationToken token);
    }
    
    internal sealed class SetLocationService: AssetServiceBase, ISetLocationService
    {
        private const string END_POINT = "SetLocation";
        public SetLocationService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }
        
        public async Task<Result<SetLocationFullInfo>> GetSetLocationAsync(long id, CancellationToken token)
        {
            try
            {
                return await GetSetLocationAsyncInternal(id, token);
            }
            catch (OperationCanceledException)
            {
                return Result<SetLocationFullInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, long? setLocationCategoryId = null, string filter = null, long? taskId = null, CancellationToken token = default)
        {
            try
            {
                return await GetSetLocationListInternalAsync(target, takeNext, takePrevious, setLocationCategoryId, filter, taskId, false, raceId, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<SetLocationFullInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListAsync(SetLocationFilterModel filter, CancellationToken token = default)
        {
            try
            {
                return await GetSetLocationListInternalAsync(filter, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<SetLocationFullInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<SetLocationFullInfo>> GetMySetLocationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default)
        {
            try
            {
                return await GetMySetLocationListInternalAsync(target, takeNext, takePrevious, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<SetLocationFullInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<SetLocationFullInfo>> GetVideoMessageSetLocationListAsync(long? target, int takeNext, int takePrevious, long raceId, CancellationToken token)
        {
            try
            {
                return await GetSetLocationListInternalAsync(target, takeNext, takePrevious, null, null, null, true, raceId, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<SetLocationFullInfo>.Cancelled();
            }
        }
        
        public async Task<ArrayResult<SetLocationBackground>> GetSetLocationBackgroundListAsync(int take, int skip, CancellationToken token)
        {
            try
            {
                var url = BuildUrl($"{END_POINT}/background?take={take}&skip={skip}");
                return await SendRequestForListModels<SetLocationBackground>(url, token);

            }
            catch (OperationCanceledException)
            {
                return ArrayResult<SetLocationBackground>.Cancelled();
            }
        }

        public async Task<Result<BackgroundOptions>> GetSetLocationBackgroundOptionsAsync(int take, int skip, CancellationToken token)
        {
            try
            {
                var url = BuildUrl($"{END_POINT}/background-and-settings?take={take}&skip={skip}");
                return await SendRequestForSingleModel<BackgroundOptions>(url, token);

            }
            catch (OperationCanceledException)
            {
                return Result<BackgroundOptions>.Cancelled();
            }
        }

        private Task<Result<SetLocationFullInfo>> GetSetLocationAsyncInternal(long id, CancellationToken token)
        {
            var url = BuildUrl($"{END_POINT}/{id}");
            return SendRequestForSingleModel<SetLocationFullInfo>(url, token);
        }
        
        private Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListInternalAsync(long? target, int takeNext, int takePrevious, long? categoryId, string filter, long? taskId, bool? forVideoMessageOnly, long raceId, CancellationToken token)
        {
            var body = new SetLocationFilterModel
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious,
                SetLocationCategoryId = categoryId,
                Name = filter,
                TaskId = taskId,
                ForVideoMessageOnly = forVideoMessageOnly,
                UnityVersion = UnityConstants.UnityVersion,
                RaceId = raceId
            };
            return GetSetLocationListInternalAsync(body, token);
        }

        private Task<ArrayResult<SetLocationFullInfo>> GetSetLocationListInternalAsync(SetLocationFilterModel filterModel, CancellationToken token)
        {
            var url = BuildUrl(END_POINT);
            return SendRequestForListModels<SetLocationFullInfo>(url, token, filterModel);
        }

        private Task<ArrayResult<SetLocationFullInfo>> GetMySetLocationListInternalAsync(long? target, int takeNext, int takePrevious, CancellationToken token)
        {
            var url = BuildUrl($"{END_POINT}/my");
            var body = new
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious
            };
            return SendRequestForListModels<SetLocationFullInfo>(url, token, body);
        }
    }
    
    public class SetLocationFilterModel
    {
        public long RaceId { get; set; }

        public long? SetLocationCategoryId { get; set; }

        public string Name { get; set; }

        public long? TaskId { get; set; }

        public long[] TagIds { get; set; }

        public bool? ForVideoMessageOnly { get; set; }

        public string UnityVersion { get; set; }

        public long? Target { get; set; }

        public int TakePrevious { get; set; }

        public int TakeNext { get; set; } = 20;
    }
}
