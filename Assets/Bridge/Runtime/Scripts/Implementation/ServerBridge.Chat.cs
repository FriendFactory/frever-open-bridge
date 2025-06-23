using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer;
using Bridge.Models.ClientServer.Chat;
using Bridge.Results;
using Bridge.Scripts.ClientServer.Chat;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ChatCreationResult> CreateChat(SaveChatModel model)
        {
            return _chatService.CreateChat(model);
        }

        public Task<Result> UpdateChat(long chatId, SaveChatModel model)
        {
            return _chatService.UpdateChat(chatId, model);
        }

        public Task<Result> InviteChatMembers(InviteMembersModel model)
        {
            return _chatService.InviteChatMembers(model);
        }

        public Task<Result> LeaveChat(long chatId)
        {
            return _chatService.LeaveChat(chatId);
        }

        public Task<ArrayResult<ChatShortInfo>> GetMyChats(int skip, int take, CancellationToken token = default)
        {
            return _chatService.GetMyChats(skip, take, token);
        }

        public Task<Result<ChatInfo>> GetChatById(long id, CancellationToken token = default)
        {
            return _chatService.GetChatById(id, token);
        }

        public Task<Result<ChatInfo>> GetDirectMessageChatByGroupId(long groupId, CancellationToken token = default)
        {
            return _chatService.GetDirectMessageChatByGroupId(groupId, token);
        }

        public Task<CountResult> GetChatUnreadMessagesCount(CancellationToken token = default)
        {
            return _chatService.GetChatUnreadMessagesCount(token);
        }

        public Task<ArrayResult<ChatMessage>> GetChatMessages(long chatId, long? target, int takeOlder, int takeNewer, CancellationToken token = default)
        {
            return _chatService.GetChatMessages(chatId, target, takeOlder, takeNewer, token);
        }

        public Task<ArrayResult<MessageReport>> GetMessageReports(long chatId, int skip, int take, CancellationToken token = default)
        {
            return _chatService.GetMessageReports(chatId, skip, take, token);
        }

        public Task<ArrayResult<ReportReason>> GetMessageReportReasons(CancellationToken token = default)
        {
            return _chatService.GetMessageReportReasons(token);
        }

        public Task<Result> LikeMessage(long chatId, long messageId)
        {
            return _chatService.LikeMessage(chatId, messageId);
        }

        public Task<Result> UnlikeMessage(long chatId, long messageId)
        {
            return _chatService.UnlikeMessage(chatId, messageId);
        }

        public Task<Result> PostMessage(long chatId, AddMessageModel model)
        {
            return _chatService.PostMessage(chatId, model);
        }
        
        public Task<Result> PostMessage(SendMessageToGroupsModel model)
        {
            return _chatService.PostMessage(model);
        }

        public Task<Result> ReportMessage(long chatId, ReportMessageModel model)
        {
            return _chatService.ReportMessage(chatId, model);
        }

        public Task<Result> SetReportMessageHidden(long chatId, long reportId, bool isHidden)
        {
            return _chatService.SetReportMessageHidden(chatId, reportId, isHidden);
        }

        public Task<Result> CloseMessageReport(long chatId, long reportId, bool isClosed)
        {
            return _chatService.CloseMessageReport(chatId, reportId, isClosed);
        }

        public Task<Result> DeleteMessage(long chatId, long messageId)
        {
            return _chatService.DeleteMessage(chatId, messageId);
        }

        public Task<Result> MuteChatNotifications(long chatId, MuteChatTimeOptions option)
        {
            return _chatService.MuteChatNotifications(chatId, option);
        }
    }
}