using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Invitation
{
    interface IInvitationService
    {
        Task<Result<InvitationCode>> GetInvitationCode(CancellationToken token);
        Task<Result<InviteeReward>> UseInvitationCode(Guid invitationGuid);
        Task<Result> SaveInvitationCode(string code);
        Task<Result<InviteeReward>> GetUnclaimedInviteeReward(CancellationToken token);
        Task<Result<string>> GetCreatorCode(CancellationToken token);
        Task<Result<StarCreator>> UseCreatorCode(string code);
        Task<Result> DeleteCreatorCode();
        Task<CountResult> GetCreatorAcceptedInvitationsCount(CancellationToken token);
    }

    internal sealed class InvitationService: ServiceBase, IInvitationService
    {
        private const string INVITATION_END_POINT = "invitation-code";
        private const string CREATOR_CODE_END_POINT = "creator-code";
        
        public InvitationService(string host, IRequestHelper requestHelper, ISerializer serializer) 
            : base(host, requestHelper, serializer)
        {
        }

        public async Task<Result<InvitationCode>> GetInvitationCode(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{INVITATION_END_POINT}/my");
                return await SendRequestForSingleModel<InvitationCode>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<InvitationCode>.Cancelled();
            }
        }

        public async Task<Result<InviteeReward>> UseInvitationCode(Guid invitationGuid)
        {
            var url = ConcatUrl(Host, $"{INVITATION_END_POINT}/usage/{invitationGuid}");
            return await SendPostRequest<InviteeReward>(url);
        }

        public Task<Result> SaveInvitationCode(string code)
        {
            var url = ConcatUrl(Host, $"{INVITATION_END_POINT}/my/{code}");
            return SendPostRequest(url);
        }

        public async Task<Result<InviteeReward>> GetUnclaimedInviteeReward(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{INVITATION_END_POINT}/invitee/reward");
                return await SendRequestForSingleModel<InviteeReward>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<InviteeReward>.Cancelled();
            }
        }

        public async Task<Result<string>> GetCreatorCode(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{CREATOR_CODE_END_POINT}/my");
                return await SendRequestForSingleModel<string>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<string>.Cancelled();
            }
        }

        public Task<Result<StarCreator>> UseCreatorCode(string code)
        {
            var url = ConcatUrl(Host, $"{CREATOR_CODE_END_POINT}/usage/{code}");
            return SendPostRequest<StarCreator>(url);
        }

        public async Task<CountResult> GetCreatorAcceptedInvitationsCount(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{CREATOR_CODE_END_POINT}/my/supporters/count");
                return await SendRequestForCountModel(url, token);
            }
            catch (OperationCanceledException)
            {
                return CountResult.Cancelled();
            }
        }

        public Task<Result> DeleteCreatorCode()
        {
            var url = ConcatUrl(Host, $"{CREATOR_CODE_END_POINT}/usage");
            return SendDeleteRequest(url);
        }
    }
}