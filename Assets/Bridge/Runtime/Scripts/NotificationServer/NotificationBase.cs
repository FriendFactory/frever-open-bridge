using System;
using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.NotificationServer
{
    public class NotificationBase
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime? Expires { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool HasRead { get; set; }
    }

    public class VideoNotificationBase: NotificationBase
    {
        public EventInfo Event { get; set; }
    }
}