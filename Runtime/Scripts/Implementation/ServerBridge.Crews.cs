using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.ClientServer.Crews;
using Bridge.Results;

namespace Bridge
{
    public partial class ServerBridge
    {
        public Task<Result<CrewModel>> GetCrew(long crewId, CancellationToken cancellationToken)
        {
            return _crewsService.GetCrew(crewId, cancellationToken);
        }

        public Task<ArrayResult<CrewMember>> GetCrewMembersTopList(long crewId, CancellationToken cancellationToken)
        {
            return _crewsService.GetCrewMembersTopList(crewId, cancellationToken);
        }

        public Task<ArrayResult<CrewShortInfo>> GetCrewsList(string filter, int takeNext, int skip, IDictionary<string,string> headers, CancellationToken cancellationToken)
        {
            return _crewsService.GetCrewsList(filter, takeNext, skip, headers, cancellationToken);
        }

        public Task<ArrayResult<CrewTopInfo>> GetCrewsTopList(string filter, int takeNext, int skip, string date, IDictionary<string, string> headers,
            CancellationToken cancellationToken)
        {
            return _crewsService.GetCrewsTopList(filter, takeNext, skip, date, headers, cancellationToken);
        }

        public Task<ArrayResult<CrewMemberRequest>> GetJoinRequests(long crewId, CancellationToken cancellationToken)
        {
            return _crewsService.GetJoinRequests(crewId, cancellationToken);
        }

        public Task<ArrayResult<long>> GetInvitations(CancellationToken cancellationToken)
        {
            return _crewsService.GetInvitations(cancellationToken);
        }

        public Task<Result> SendJoinRequest(long crewId, string userPitch)
        {
            return _crewsService.SendJoinRequest(crewId, userPitch);
        }

        public Task<Result> LeaveCrew(long crewId, long? newLeaderId = null)
        {
            return _crewsService.LeaveCrew(crewId, newLeaderId);
        }

        public Task<Result> AcceptCrewInvitation(long crewId)
        {
            return _crewsService.AcceptCrewInvitation(crewId);
        }

        public Task<Result> IgnoreCrewInvitation(long crewId)
        {
            return _crewsService.IgnoreCrewInvitation(crewId);
        }

        public Task<Result> AcceptJoinCrewRequest(long crewId, long userId)
        {
            return _crewsService.AcceptJoinCrewRequest(crewId, userId);
        }

        public Task<Result> IgnoreJoinCrewRequest(long crewId, long userId)
        {
            return _crewsService.IgnoreJoinCrewRequest(crewId, userId);
        }

        public Task<Result> InviteUsersToCrew(long crewId, long[] userIds)
        {
            return _crewsService.InviteUsersToCrew(crewId, userIds);
        }

        public Task<Result> UpdateCrewMemberRole(long crewId, long userId, long roleId)
        {
            return _crewsService.UpdateCrewMemberRole(crewId, userId, roleId);
        }

        public Task<Result> RemoveCrewMember(long crewId, long userGroupId)
        {
            return _crewsService.RemoveCrewMember(crewId, userGroupId);
        }

        public Task<Result<CrewModel>> CreateCrew(SaveCrewModel model)
        {
            return _crewsService.CreateCrew(model);
        }

        public Task<Result> SetMessageOfTheDay(long crewId, string message)
        {
            return _crewsService.SetMessageOfTheDay(crewId, message);
        }

        public Task<Result<CrewModel>> UpdateCrewData(long crewId, SaveCrewModel model)
        {
            return _crewsService.UpdateCrewData(crewId, model);
        }

        public Task<ArrayResult<long>> GetInvitedUsers(long crewId, CancellationToken cancellationToken)
        {
            return _crewsService.GetInvitedUsers(crewId, cancellationToken);
        }

        public Task<Result<ValidationResponse>> ValidateCrewName(string name)
        {
            return _crewsService.ValidateCrewName(name);
        }

        public Task<Result<ChatShortInfo>> GetMyCrewChat(CancellationToken cancellationToken)
        {
            return _crewsService.GetMyCrewChat(cancellationToken);
        }
    }
}