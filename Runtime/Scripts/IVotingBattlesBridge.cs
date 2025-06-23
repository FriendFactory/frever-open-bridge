using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Battles;
using Bridge.Results;

namespace Bridge
{
    public interface IVotingBattlesBridge
    {
        Task<ArrayResult<Battle>> GetVotingBattlePairs(long taskId, CancellationToken token = default);

        Task<Result> Vote(long taskId, ICollection<BattleVoteModel> models);

        Task<ArrayResult<BattleResult>> GetVotingBattleResult(long taskId, CancellationToken token = default);
        
        Task<ClaimRewardResult> ClaimVotingBattleReward(long taskId);
        
        Task<ClaimedStatusResult> GetBattleRewardStatus(long taskId, CancellationToken token = default);
    }
}