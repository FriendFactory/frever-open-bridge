using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Constants;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.BodyAnimations
{
    public interface IBodyAnimationService
    {
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long raceId, string filter = null, long? categoryId = null,long? taskId = null, int? characterCount = null, long? emotionId = null, long[] movementTypeIds = null, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long movementTypeId, int characterCount, long raceId, string filter = null, 
            long? taskId = null, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(BodyAnimationFilterModel filterModel,
            CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetMyBodyAnimationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationGroupAsync(long id, CancellationToken token = default);
        Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationByIdsAsync(long[] ids, CancellationToken token = default);
        Task<Result<BodyAnimationInfo>> GetBodyAnimationAsync(long id, CancellationToken token = default);
    }
    
    internal sealed class BodyAnimationService : AssetServiceBase, IBodyAnimationService
    {
        private const string END_POINT = "BodyAnimation";
        
        public BodyAnimationService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long raceId, string filter = null, long? categoryId = null, long? taskId = null, int? characterCount = null, long? emotionId = null, long[] movementTypeIds = null, CancellationToken token = default)
        {
            try
            {
                return await BodyAnimationListAsyncInternal(target, takeNext, takePrevious, filter, categoryId, taskId, characterCount, emotionId, raceId, movementTypeIds, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default)
        {
            try
            {
                return await BodyAnimationListAsyncInternal(filterModel, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }
        
        public async Task<ArrayResult<BodyAnimationInfo>> GetMyBodyAnimationListAsync(long? target, int takeNext, int takePrevious, CancellationToken token = default)
        {
            try
            {
                return await GetMyBodyAnimationListInternalAsync(target, takeNext, takePrevious, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationGroupAsync(long id, CancellationToken token)
        {
            try
            {
                return await GeBodyAnimationGroupInternalAsync(id, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<BodyAnimationInfo>> GetBodyAnimationByIdsAsync(long[] ids, CancellationToken token = default)
        {
            try
            {
                var url = BuildUrl($"{END_POINT}/by-ids");
                return await SendRequestForListModels<BodyAnimationInfo>(url, token, ids);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(long? target, int takeNext, int takePrevious, long movementTypeId,
            int characterCount, long raceId, string filter = null, long? taskId = null, CancellationToken token = default)
        {
            try
            {
                return await RecommendedBodyAnimationListAsyncInternal(target, takeNext, takePrevious, movementTypeId,
                    characterCount, filter, taskId, raceId, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }
        
        
        
        public async Task<ArrayResult<BodyAnimationInfo>> GetRecommendedBodyAnimationListAsync(BodyAnimationFilterModel filterModel, CancellationToken token = default)
        {
            try
            {
                return await RecommendedBodyAnimationListAsyncInternal(filterModel, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<BodyAnimationInfo>.Cancelled();
            }
        }

        public async Task<Result<BodyAnimationInfo>> GetBodyAnimationAsync(long id, CancellationToken token)
        {
            try
            {
                var url = BuildUrl($"{END_POINT}/{id}");
                return await SendRequestForSingleModel<BodyAnimationInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<BodyAnimationInfo>.Cancelled();
            }
        }

        private Task<ArrayResult<BodyAnimationInfo>> BodyAnimationListAsyncInternal(long? target, int takeNext, int takePrevious, string filter, long? categoryId, long? taskId, int? characterCount, long? emotionId, long raceId, long[] movementTypeIds, CancellationToken token)
        {
            var body = new BodyAnimationFilterModel
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious,
                Name = filter,
                BodyAnimationCategoryId = categoryId,
                TaskId = taskId,
                CharacterCount = characterCount,
                EmotionId = emotionId,
                UnityVersion = UnityConstants.UnityVersion,
                RaceId = raceId,
                MovementTypeIds = movementTypeIds
            };
            return BodyAnimationListAsyncInternal(body, token);
        }
        
        private Task<ArrayResult<BodyAnimationInfo>> BodyAnimationListAsyncInternal(BodyAnimationFilterModel filterModel, CancellationToken token)
        {
            var url = BuildUrl(END_POINT);
            return SendRequestForListModels<BodyAnimationInfo>(url, token, filterModel);
        }
        
        private Task<ArrayResult<BodyAnimationInfo>> RecommendedBodyAnimationListAsyncInternal(long? target, int takeNext, int takePrevious, long movementTypeId, int characterCount, string filter, long? taskId, long raceId, CancellationToken token)
        {
            var body = new BodyAnimationFilterModel
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious,
                Name = filter,
                TaskId = taskId,
                CharacterCount = characterCount,
                MovementTypeIds = new [] { movementTypeId },
                UnityVersion = UnityConstants.UnityVersion,
                RaceId = raceId
            };
            return RecommendedBodyAnimationListAsyncInternal(body, token);
        }

        private Task<ArrayResult<BodyAnimationInfo>> RecommendedBodyAnimationListAsyncInternal(BodyAnimationFilterModel filterModel, CancellationToken  token)
        {
            var url = BuildUrl($"{END_POINT}/recommended");
            return SendRequestForListModels<BodyAnimationInfo>(url, token, filterModel);
        }

        private Task<ArrayResult<BodyAnimationInfo>> GetMyBodyAnimationListInternalAsync(long? target, int takeNext, int takePrevious,
            CancellationToken token = default)
        {
            var url = BuildUrl($"{END_POINT}/my");
            var body = new
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious
            };
            return SendRequestForListModels<BodyAnimationInfo>(url, token, body);
        }
        
        private Task<ArrayResult<BodyAnimationInfo>> GeBodyAnimationGroupInternalAsync(long id, CancellationToken token = default)
        {
            var url = BuildUrl($"{END_POINT}/group/{id}");
            return SendRequestForListModels<BodyAnimationInfo>(url, token);
        }
    }
    
    public sealed class BodyAnimationFilterModel
    {
        public long RaceId { get; set; }

        public long? BodyAnimationCategoryId { get; set; }
        
        public long[] MovementTypeIds { get; set; }

        public string Name { get; set; }

        public long? TaskId { get; set; }

        public long[] TagIds { get; set; }

        public long? EmotionId { get; set; }

        public string UnityVersion { get; set; }

        public long? Target { get; set; }

        public int TakePrevious { get; set; }

        public int TakeNext { get; set; } = 20;

        public int? CharacterCount { get; set; }

        public long? BodyAnimationGroupId { get; set; }
    }
}
