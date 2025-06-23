using System.Threading;
using System.Threading.Tasks;
using Bridge.NotificationServer;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ArrayResult<NotificationBase>> MyLatestNotifications(int? top, CancellationToken token)
        {
            return _notificationService.MyLatestNotifications(top, token);
        }

        public Task<Result> MarkNotificationsAsRead(long[] notificationIds)
        {
            return _notificationService.MarkAsRead(notificationIds);
        }
    }
}