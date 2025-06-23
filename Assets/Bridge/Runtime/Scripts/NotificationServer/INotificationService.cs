using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.NotificationServer
{
    public interface INotificationService
    {
        Task<ArrayResult<NotificationBase>> MyLatestNotifications(int? top = null, CancellationToken token = default);

        Task<Result> MarkAsRead(long[] notificationIds);
    }
}