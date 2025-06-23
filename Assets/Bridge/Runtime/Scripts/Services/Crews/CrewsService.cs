using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Authorization.Models;
using Bridge.ClientServer;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.ClientServer.Crews;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.Services.Crews
{
    internal sealed class CrewsService : ServiceBase, ICrewsService
    {
        internal CrewsService(string serviceUrl, IRequestHelper requestHelper, ISerializer serializer) : base(
            serviceUrl, requestHelper, serializer)
        {
        }

        public async Task<Result<CrewModel>> GetCrew(long crewId, CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}");
                return await SendRequestForSingleModel<CrewModel>(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<CrewModel>.Cancelled()
                    : Result<CrewModel>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<CrewMember>> GetCrewMembersTopList(long crewId,
            CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/members/top-list");
                return await SendRequestForListModels<CrewMember>(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<CrewMember>.Cancelled()
                    : ArrayResult<CrewMember>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<CrewShortInfo>> GetCrewsList(string filter, int take, int skip,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew?filter={filter}&take={take}&skip={skip}");
                return await SendRequestForListModels<CrewShortInfo>(url, cancellationToken, headers:headers);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<CrewShortInfo>.Cancelled()
                    : ArrayResult<CrewShortInfo>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<CrewTopInfo>> GetCrewsTopList(string filter, int takeNext, int skip, string date, IDictionary<string, string> headers,
            CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/top?filter={filter}&take={takeNext}&skip={skip}&date={date}");
                return await SendRequestForListModels<CrewTopInfo>(url, cancellationToken, headers:headers);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<CrewTopInfo>.Cancelled()
                    : ArrayResult<CrewTopInfo>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<CrewMemberRequest>> GetJoinRequests(long crewId,
            CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/requests");
                return await SendRequestForListModels<CrewMemberRequest>(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<CrewMemberRequest>.Cancelled()
                    : ArrayResult<CrewMemberRequest>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<long>> GetInvitations(CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, "crew/invitations");
                return await SendRequestForListModels<long>(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<long>.Cancelled()
                    : ArrayResult<long>.Error(e.Message);
            }
        }

        public async Task<Result> SendJoinRequest(long crewId, string userPitch = null)
        {
            try
            {
                var url = ConcatUrl(Host, "crew/join");
                var body = new JoinCrewRequest
                {
                    CrewId = crewId,
                    UserPitch = userPitch
                };
                return await SendPostRequest(url, body);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> LeaveCrew(long crewId, long? newLeaderId = null)
        {
            try
            {
                var url = ConcatUrl(Host, "crew/leave");
                var body = new LeaveCrewRequest
                {
                    CrewId = crewId,
                    NewLeaderGroupId = newLeaderId
                };
                return await SendPostRequest(url, body);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> IgnoreCrewInvitation(long crewId)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/invitation/ignore");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> AcceptCrewInvitation(long crewId)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/invitation/approve");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> AcceptJoinCrewRequest(long crewId, long userId)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/request/{userId}/approve");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> IgnoreJoinCrewRequest(long crewId, long userId)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/request/{userId}/ignore");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> InviteUsersToCrew(long crewId, long[] userIds)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/invitations");
                return await SendPostRequest(url, userIds);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> UpdateCrewMemberRole(long crewId, long userId, long roleId)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/member/{userId}/role/{roleId}");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> RemoveCrewMember(long crewId, long userGroupId)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/member/{userGroupId}");
                return await SendDeleteRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result<CrewModel>> CreateCrew(SaveCrewModel model)
        {
            try
            {
                var url = ConcatUrl(Host, "crew");
                return await SendPostRequest<CrewModel>(url, model);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<CrewModel>.Cancelled()
                    : Result<CrewModel>.Error(e.Message);
            }
        }

        public async Task<Result> SetMessageOfTheDay(long crewId, string message)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/message-of-day");
                return await SendPostRequest(url, message);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result<CrewModel>> UpdateCrewData(long crewId, SaveCrewModel model)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}");
                return await SendPutRequest<CrewModel>(url, model);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<CrewModel>.Cancelled()
                    : Result<CrewModel>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<long>> GetInvitedUsers(long crewId, CancellationToken cancellationToken)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/{crewId}/invited/groups");
                return await SendRequestForListModels<long>(url, cancellationToken);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<long>.Cancelled()
                    : ArrayResult<long>.Error(e.Message);
            }
        }

        public async Task<Result<ValidationResponse>> ValidateCrewName(string name)
        {
            try
            {
                var url = ConcatUrl(Host, $"crew/validate?name={name}");
                return await SendPostRequest<ValidationResponse>(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<ValidationResponse>.Cancelled()
                    : Result<ValidationResponse>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<LanguageInfo>> GetAvailableLanguages(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, "language/crew");
                return await SendRequestForListModels<LanguageInfo>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? ArrayResult<LanguageInfo>.Cancelled()
                    : ArrayResult<LanguageInfo>.Error(e.Message);
            }
        }

        public async Task<Result<ChatShortInfo>> GetMyCrewChat(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, "crew/my/chat");
                return await SendRequestForSingleModel<ChatShortInfo>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<ChatShortInfo>.Cancelled()
                    : Result<ChatShortInfo>.Error(e.Message);
            }
        }
    }
}