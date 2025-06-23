using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Tasks;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Tasks
{
    internal interface ITaskService
    {
        Task<ArrayResult<TaskInfo>> GetTasksAsync(long? targetTaskId, int takeNext, int takePrevious, string filter, TaskType? taskType, CancellationToken token);

        Task<ArrayResult<TaskInfo>> GetJoinedVotingTasks(long? targetTaskId, int takeNext, int takePrevious,
            CancellationToken token);
        
        Task<CountResult> GetJoinedVotingTasksCount(CancellationToken token);

        Task<NextTaskReleaseDateResult> GetNextTaskReleaseDate(CancellationToken token);
        
        Task<ArrayResult<TaskInfo>> GetTrendingTasksAsync(long? targetTaskId, int takeNext, int takePrevious,
            CancellationToken token);

        Task<ArrayResult<TaskInfo>> GetUserTasksAsync(long userGroupId, long? targetTaskId, int takeNext, int takePrevious, CancellationToken token);

        Task<Result<TaskFullInfo>> GetTaskFullInfoAsync(long taskId, CancellationToken token);
        
        Task<Result<LevelFullData>> GetLevelForTaskAsync(long taskId, CancellationToken token);
    }

    internal sealed class TaskService : ServiceBase, ITaskService
    {
        private const string END_POINT = "task";
        
        public TaskService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }
        
        public async Task<ArrayResult<TaskInfo>> GetTasksAsync(long? targetTaskId, int takeNext, int takePrevious, string filter, TaskType? taskType, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}?&takeNext={takeNext}&takePrevious={takePrevious}");
                if (!string.IsNullOrEmpty(filter))
                {
                    url += $"&filter={filter}";
                }

                if (taskType.HasValue)
                {
                    url += $"&type={(int)taskType}";
                }
                AddTargetId(targetTaskId, ref url);
                return await SendRequestForListModels<TaskInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<TaskInfo>();
            }
        }

        public async Task<ArrayResult<TaskInfo>> GetJoinedVotingTasks(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/voting/my?&takeNext={takeNext}&takePrevious={takePrevious}");
                AddTargetId(targetTaskId, ref url);
                return await SendRequestForListModels<TaskInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<TaskInfo>();
            }
        }

        public async Task<CountResult> GetJoinedVotingTasksCount(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/voting/my/count");
                return await SendRequestForCountModel(url, token);
            }
            catch (OperationCanceledException)
            {
                return CountResult.Cancelled();
            }
        }

        public async Task<NextTaskReleaseDateResult> GetNextTaskReleaseDate(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/available/time");
                var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
                var resp = await req.GetHTTPResponseAsync(token);
                if (resp.IsSuccess)
                {
                    var date = Serializer.DeserializeJson<DateTime>(resp.DataAsText);
                    return NextTaskReleaseDateResult.Success(date);
                }
                
                if (resp.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    return NextTaskReleaseDateResult.DontHaveNextTaskReleaseDateResult();
                }

                return NextTaskReleaseDateResult.Error(resp.DataAsText);
            }
            catch (OperationCanceledException)
            {
                return NextTaskReleaseDateResult.Cancelled();
            }
        }

        public async Task<ArrayResult<TaskInfo>> GetTrendingTasksAsync(long? targetTaskId, int takeNext, int takePrevious, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/trending?&takeNext={takeNext}&takePrevious={takePrevious}");
                AddTargetId(targetTaskId, ref url);
                return await SendRequestForListModels<TaskInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<TaskInfo>();
            }
        }

        private static void AddTargetId(long? targetTaskId, ref string url)
        {
            if (targetTaskId.HasValue)
            {
                url += $"&target={targetTaskId.Value}";
            }
        }

        public async Task<ArrayResult<TaskInfo>> GetUserTasksAsync(long userGroupId, long? targetTaskId, int takeNext, int takePrevious, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/by-group/{userGroupId}?&takeNext={takeNext}&takePrevious={takePrevious}");
                AddTargetId(targetTaskId, ref url);
                return await SendRequestForListModels<TaskInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<TaskInfo>();
            }
        }

        public async Task<Result<TaskFullInfo>> GetTaskFullInfoAsync(long taskId, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/{taskId}");
                return await SendRequestForSingleModel<TaskFullInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<TaskFullInfo>.Cancelled();
            }
        }

        public async Task<Result<LevelFullData>> GetLevelForTaskAsync(long taskId, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/{taskId}/level");
                return await SendRequestForSingleModel<LevelFullData>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelFullData>.Cancelled();
            }
        }
    }
}
