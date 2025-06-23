using System.Threading;
using System.Threading.Tasks;
using Bridge.NotificationServer;
using Bridge.Results;

namespace Bridge
{
    public interface INotificationsBridge
    {
        Task<ArrayResult<NotificationBase>> MyLatestNotifications(int? top, CancellationToken token = default);
        Task<Result> MarkNotificationsAsRead(long[] notificationIds);
    }
}