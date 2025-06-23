using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Battles;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Battles
{
    internal interface IBattleService
    {
        Task<ArrayResult<Battle>> GetVotingBattlePairs(long taskId, CancellationToken token);
        Task<Result> Vote(long taskId, ICollection<BattleVoteModel> models);
        Task<ArrayResult<BattleResult>> GetVotingBattleResult(long taskId, CancellationToken token);
        Task<ClaimedStatusResult> GetBattleRewardStatus(long taskId, CancellationToken token);
    }

    internal sealed class BattleService : ServiceBase, IBattleService
    {
        private const string END_POINT = "battle";
        
        public BattleService(string host, IRequestHelper requestHelper, ISerializer serializer) 
            : base(host, requestHelper, serializer)
        {
        }

        public Task<ArrayResult<Battle>> GetVotingBattlePairs(long taskId, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/{taskId}");
            return SendRequestForListModels<Battle>(url, token);
        }

        public Task<Result> Vote(long taskId, ICollection<BattleVoteModel> models)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/{taskId}/vote");
            return SendPostRequest(url, models);
        }

        public Task<ArrayResult<BattleResult>> GetVotingBattleResult(long taskId, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/{taskId}/result");
            return SendRequestForListModels<BattleResult>(url, token);
        }

        public async Task<ClaimedStatusResult> GetBattleRewardStatus(long taskId, CancellationToken token)
        {
            try
            {
                return await GetBattleRewardStatusInternal(taskId, token);
            }
            catch (OperationCanceledException)
            {
                return ClaimedStatusResult.Cancelled();
            }
        }
        
        private async Task<ClaimedStatusResult> GetBattleRewardStatusInternal(long taskId, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/{taskId}/reward/claimed");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await req.GetHTTPResponseAsync(token: token);

            if (token.IsCancellationRequested)
            {
                return ClaimedStatusResult.Cancelled();
            }
            
            if (!resp.IsSuccess)
            {
                return ClaimedStatusResult.Error(resp.DataAsText, resp.StatusCode);
            }

            var isClaimed = Serializer.DeserializeJson<bool>(resp.DataAsText);
            return ClaimedStatusResult.Success(isClaimed);
        }
    }
}
