using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.SocialActions;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.SocialActions
{
    public interface ISocialActionsService
    {
        Task<ArrayResult<SocialActionFullInfo>> GetPersonalisedSocialActions(string treatmentGroup, IDictionary<string, string> headers, CancellationToken token);
        Task<Result> DeleteAction(Guid recommendationId, long actionId);
        Task<Result> MarkActionAsComplete(Guid recommendationId, long actionId);
    }

    internal sealed class SocialActionsService : ServiceBase, ISocialActionsService
    {
        private const string END_POINT = "social-action";

        public SocialActionsService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host,
            requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<SocialActionFullInfo>> GetPersonalisedSocialActions(string treatmentGroup, IDictionary<string, string> headers,  CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/personal?treatmentGroup={treatmentGroup}");

                return await SendRequestForListModels<SocialActionFullInfo>(url, token, headers:headers);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<SocialActionFullInfo>();
            }
        }

        public async Task<Result> DeleteAction(Guid recommendationId, long actionId)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/cancel/{recommendationId}/{actionId}");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            var response = await request.GetHTTPResponseAsync();
            if (!response.IsSuccess)
            {
                return new ErrorResult(response.DataAsText);
            }

            return new SuccessResult();
        }

        public async Task<Result> MarkActionAsComplete(Guid recommendationId, long actionId)
        {
            var url = ConcatUrl(Host, $"{END_POINT}/complete/{recommendationId}/{actionId}");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var response = await request.GetHTTPResponseAsync();
            if (!response.IsSuccess)
            {
                return new ErrorResult(response.DataAsText);
            }

            return new SuccessResult();
        }
    }
}