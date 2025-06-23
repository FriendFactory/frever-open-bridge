using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Chat;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Scripts.ClientServer.Chat;

namespace Bridge.ClientServer.Chat
{
    public interface IChatService
    {
        Task<ChatCreationResult> CreateChat(SaveChatModel model);
        Task<Result> UpdateChat(long chatId, SaveChatModel model);
        Task<Result> InviteChatMembers(InviteMembersModel model);
        Task<Result> LeaveChat(long chatId);
        Task<ArrayResult<ChatShortInfo>> GetMyChats(int skip, int take, CancellationToken token = default);
        Task<Result<ChatInfo>> GetChatById(long id, CancellationToken token = default); 
        Task<Result<ChatInfo>> GetDirectMessageChatByGroupId(long groupId, CancellationToken token = default); 
        Task<CountResult> GetChatUnreadMessagesCount(CancellationToken token = default); 
        Task<ArrayResult<ChatMessage>> GetChatMessages(long chatId, long? target, int takeOlder, int takeNewer, CancellationToken token = default);
        Task<ArrayResult<MessageReport>> GetMessageReports(long chatId, int skip, int take, CancellationToken token = default);
        Task<ArrayResult<ReportReason>> GetMessageReportReasons(CancellationToken token = default);
        Task<Result> LikeMessage(long chatId, long messageId);
        Task<Result> UnlikeMessage(long chatId, long messageId);
        Task<Result> PostMessage(long chatId, AddMessageModel model);
        Task<Result> PostMessage(SendMessageToGroupsModel model);
        Task<Result> ReportMessage(long chatId, ReportMessageModel model);
        Task<Result> SetReportMessageHidden(long chatId, long reportId, bool isHidden);
        Task<Result> CloseMessageReport(long chatId, long reportId, bool isClosed);
        Task<Result> DeleteMessage(long chatId, long messageId);
        Task<Result> MuteChatNotifications(long chatId, MuteChatTimeOptions option);
    }

    internal sealed class ChatService : ServiceBase, IChatService
    {
        private readonly string _endPoint;
        private readonly PostMessageService _postMessageService;
        
        public ChatService(string host, IRequestHelper requestHelper, ISerializer serializer, PostMessageService postMessageService, string endPoint) : base(host, requestHelper, serializer)
        {
            _postMessageService = postMessageService;
            _endPoint = endPoint;
        }

        public async Task<ChatCreationResult> CreateChat(SaveChatModel model)
        {
            try
            {
                var url = ConcatUrl(Host, _endPoint);
                var idResult = await SendPostRequest<long>(url, model);
                return ChatCreationResult.Result(idResult);
            }
            catch (Exception e)
            {
                return ChatCreationResult.Error(e.Message);
            }
        }

        public async Task<Result> UpdateChat(long chatId, SaveChatModel model)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}");
                return await SendPutRequest(url, model);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> InviteChatMembers(InviteMembersModel model)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/invites");
                return await SendPostRequest(url, model);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> LeaveChat(long chatId)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}");
                return await SendDeleteRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<ArrayResult<ChatShortInfo>> GetMyChats(int skip, int take, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/my?skip={skip}&take={take}");
                return await SendRequestForListModels<ChatShortInfo>(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return ArrayResult<ChatShortInfo>.Cancelled();
                return new ArrayResult<ChatShortInfo>(e.Message);
            }
        }

        public async Task<Result<ChatInfo>> GetChatById(long id, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{id}");
                return await SendRequestForSingleModel<ChatInfo>(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) Result<ChatInfo>.Cancelled();
                return Result<ChatInfo>.Error(e.Message);
            }
        }

        public async Task<Result<ChatInfo>> GetDirectMessageChatByGroupId(long groupId, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/private/{groupId}");
                return await SendRequestForSingleModel<ChatInfo>(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) Result<ChatInfo>.Cancelled();
                return Result<ChatInfo>.Error(e.Message);
            }
        }

        public async Task<CountResult> GetChatUnreadMessagesCount(CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/message/unread/count");
                return await SendRequestForCountModel(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return CountResult.Cancelled();
                return CountResult.Error(e.Message);
            }
        }

        public async Task<ArrayResult<ChatMessage>> GetChatMessages(long chatId, long? target, int takeOlder, int takeNewer, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/message?target={target}&takeOlder={takeOlder}&takeNewer={takeNewer}");
                return await SendRequestForListModels<ChatMessage>(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return ArrayResult<ChatMessage>.Cancelled();
                return ArrayResult<ChatMessage>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<MessageReport>> GetMessageReports(long chatId, int skip, int take, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/reports?skip={skip}&take={take}");
                return await SendRequestForListModels<MessageReport>(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return ArrayResult<MessageReport>.Cancelled();
                return ArrayResult<MessageReport>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<ReportReason>> GetMessageReportReasons(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/report/reasons");
                return await SendRequestForListModels<ReportReason>(url, token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return ArrayResult<ReportReason>.Cancelled();
                return ArrayResult<ReportReason>.Error(e.Message);
            }
        }

        public async Task<Result> LikeMessage(long chatId, long messageId)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/message/{messageId}/like");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> UnlikeMessage(long chatId, long messageId)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/message/{messageId}/like");
                return await SendDeleteRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public Task<Result> PostMessage(long chatId, AddMessageModel model)
        {
            return _postMessageService.PostMessage(chatId, model);
        }

        public Task<Result> PostMessage(SendMessageToGroupsModel model)
        {
            return _postMessageService.PostMessage(model);
        }

        public async Task<Result> ReportMessage(long chatId, ReportMessageModel model)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/report");
                return await SendPostRequest(url, model);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> SetReportMessageHidden(long chatId, long reportId, bool isHidden)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/report/{reportId}/hidden/{isHidden}");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> CloseMessageReport(long chatId, long reportId, bool isClosed)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/report/{reportId}/closed/{isClosed}");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> DeleteMessage(long chatId, long messageId)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/message/{messageId}");
                return await SendDeleteRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> MuteChatNotifications(long chatId, MuteChatTimeOptions option)
        {
            try
            {
                var url = ConcatUrl(Host, $"{_endPoint}/{chatId}/mute/{(int)option}");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }
    }
}
