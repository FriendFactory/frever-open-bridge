using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer;
using Bridge.ClientServer.Invitation;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Results;

namespace Bridge
{
    public partial class ServerBridge
    {
        private IInvitationService _invitationService;
        
        public Task<Result<InvitationCode>> GetInvitationCode(CancellationToken token)
        {
            return _invitationService.GetInvitationCode(token);
        }

        public Task<Result<InviteeReward>> UseInvitationCode(Guid invitationGuid)
        {
            return _invitationService.UseInvitationCode(invitationGuid);
        }

        public Task<Result> SaveInvitationCode(string code)
        {
            return _invitationService.SaveInvitationCode(code);
        }

        public Task<Result<string>> GetCreatorCode(CancellationToken token)
        {
            return _invitationService.GetCreatorCode(token);
        }

        public Task<Result<StarCreator>> UseCreatorCode(string code)
        {
            return _invitationService.UseCreatorCode(code);
        }

        public Task<Result> DeleteCreatorCode()
        {
            return _invitationService.DeleteCreatorCode();
        }

        public Task<CountResult> GetCreatorAcceptedInvitationsCount(CancellationToken token)
        {
            return _invitationService.GetCreatorAcceptedInvitationsCount(token);
        }

        public Task<Result<InviteeReward>> GetUnclaimedInviteeReward(CancellationToken token)
        {
            return _invitationService.GetUnclaimedInviteeReward(token);
        }
    }
}