using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Recommendations;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Recommendations
{
    interface IRecommendationsService
    {
        Task<ArrayResult<FollowRecommendation>> GetFollowRecommendations(IDictionary<string,string> headers, CancellationToken token);
        Task<ArrayResult<FollowRecommendation>> GetFollowBackRecommendations(IDictionary<string,string> headers, CancellationToken token);
    }

    internal class RecommendationsService: ServiceBase, IRecommendationsService
    {
        private const string END_POINT = "group";
        
        public RecommendationsService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host,
            requestHelper, serializer) { }
        
        public async Task<ArrayResult<FollowRecommendation>> GetFollowRecommendations(IDictionary<string,string> headers, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/follow-recommendations");
                return await SendRequestForListModels<FollowRecommendation>(url, token, headers:headers);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<FollowRecommendation>.Cancelled();
            }
        }

        public async Task<ArrayResult<FollowRecommendation>> GetFollowBackRecommendations(IDictionary<string,string> headers, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/follow-back-recommendations");
                return await SendRequestForListModels<FollowRecommendation>(url, token, headers:headers);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<FollowRecommendation>.Cancelled();
            }
        }
    }
}