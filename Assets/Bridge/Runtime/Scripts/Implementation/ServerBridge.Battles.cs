using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Battles;
using Bridge.Results;

namespace Bridge
{
    public partial class ServerBridge
    {
        public Task<ArrayResult<Battle>> GetVotingBattlePairs(long taskId, CancellationToken token = default)
        {
            return _battleService.GetVotingBattlePairs(taskId, token);
        }

        public Task<Result> Vote(long taskId, ICollection<BattleVoteModel> models)
        {
            return _battleService.Vote(taskId, models);
        }

        public Task<ArrayResult<BattleResult>> GetVotingBattleResult(long taskId, CancellationToken token = default)
        {
            return _battleService.GetVotingBattleResult(taskId, token);
        }

        public Task<ClaimedStatusResult> GetBattleRewardStatus(long taskId, CancellationToken token = default)
        {
            return _battleService.GetBattleRewardStatus(taskId, token);
        }
    }
}