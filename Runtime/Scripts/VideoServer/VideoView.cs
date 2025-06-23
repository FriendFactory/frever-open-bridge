using System;

namespace Bridge.VideoServer
{
    public sealed class VideoView
    {
        public long VideoId { get; set; }

        public DateTime ViewDate { get; set; }
        public string FeedTab { get; set; }
        public string FeedType { get; set; }
    }
}