using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.ClientServer.Crews;
using Bridge.Results;

namespace Bridge.Services.Crews
{
    public interface ICrewsService
    {
        Task<ArrayResult<LanguageInfo>> GetAvailableLanguages(CancellationToken token);
        Task<Result<CrewModel>> GetCrew(long crewId, CancellationToken cancellationToken);
        Task<ArrayResult<CrewMember>> GetCrewMembersTopList(long crewId, CancellationToken cancellationToken);
        Task<ArrayResult<CrewShortInfo>> GetCrewsList(string filter, int takeNext, int skip,
            IDictionary<string, string> dictionary, CancellationToken cancellationToken);
        Task<ArrayResult<CrewTopInfo>> GetCrewsTopList(string filter, int takeNext, int skip, string date,
            IDictionary<string, string> headers, CancellationToken cancellationToken);
        Task<ArrayResult<CrewMemberRequest>> GetJoinRequests(long crewId, CancellationToken cancellationToken);
        Task<ArrayResult<long>> GetInvitations(CancellationToken cancellationToken);
        Task<Result> SendJoinRequest(long crewId, string userPitch);
        Task<Result> LeaveCrew(long crewId, long? newLeaderId = null);
        Task<Result> AcceptCrewInvitation(long crewId);
        Task<Result> IgnoreCrewInvitation(long crewId);
        Task<Result> AcceptJoinCrewRequest(long crewId, long userId);
        Task<Result> IgnoreJoinCrewRequest(long crewId, long userId);
        Task<Result> InviteUsersToCrew(long crewId, long[] userIds);
        Task<Result> UpdateCrewMemberRole(long crewId, long userId, long roleId);
        Task<Result> RemoveCrewMember(long crewId, long userGroupId);
        Task<Result<CrewModel>> CreateCrew(SaveCrewModel model);
        Task<Result> SetMessageOfTheDay(long crewId, string message);
        Task<Result<CrewModel>> UpdateCrewData(long crewId, SaveCrewModel model);
        Task<ArrayResult<long>> GetInvitedUsers(long crewId, CancellationToken cancellationToken);
        Task<Result<ValidationResponse>> ValidateCrewName(string name);
        Task<Result<ChatShortInfo>> GetMyCrewChat(CancellationToken cancellationToken = default);
    }
}