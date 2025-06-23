using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer.Recommendations;
using Bridge.Models.ClientServer.Recommendations;
using Bridge.Results;

namespace Bridge
{
    public partial class ServerBridge
    {
        private IRecommendationsService _recommendationsService;

        public Task<ArrayResult<FollowRecommendation>> GetFollowRecommendations(IDictionary<string,string> headers, CancellationToken token)
        {
            return _recommendationsService.GetFollowRecommendations(headers, token);
        }

        public Task<ArrayResult<FollowRecommendation>> GetFollowBackRecommendations(IDictionary<string,string> headers, CancellationToken token)
        {
            return _recommendationsService.GetFollowBackRecommendations(headers, token);
        }
    }
}