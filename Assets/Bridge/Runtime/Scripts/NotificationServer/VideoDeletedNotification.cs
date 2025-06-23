using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.NotificationServer
{
    public class VideoDeletedNotification: VideoNotificationBase
    {
    }

    public sealed class EventInfo: IEventInfo
    {
        public long Id { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}