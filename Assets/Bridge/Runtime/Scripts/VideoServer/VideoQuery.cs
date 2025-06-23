using System.Collections.Generic;
using Bridge.AssetManagerServer;
using Bridge.Models.VideoServer;

namespace Bridge.VideoServer
{
    internal sealed class VideoQuery : Query<Video>
    {
        private const string TARGET_VIDEO_ARG_NAME = "targetVideo";
        private const string TAKE_NEXT_ARG_NAME = "takeNext";
        private const string TAKE_PREV_ARG_NAME = "takePrevious";
        private const string SORTING_ORDER_ARG_NAME = "sorting";
        
        public long? TargetVideo;
        public int TakeNext;
        public int TakePrevious;
        public string TargetVideoKey;
        public OrderByType? SortOrder;

        public void SetStartFromVideo(string key)
        {
            TargetVideoKey = key;
        }

        public void SetStartFromVideo(long? videoId)
        {
            TargetVideo = videoId;
        }

        public void SetTakeNext(int count)
        {
            TakeNext = count;
        }

        public void SetTakePrevious(int count)
        {
            TakePrevious = count;
        }

        public void SetSortOrder(OrderByType sortOrder)
        {
            SortOrder = sortOrder;
        }
        
        internal override string BuildQuery()
        {
            var baseQuery = base.BuildQuery();
            
            if (!TargetVideo.HasValue && string.IsNullOrEmpty(TargetVideoKey))
            {
                var takeNext = $"{baseQuery}&${TAKE_NEXT_ARG_NAME}={TakeNext}";
                var url = TakePrevious <= 0 ? takeNext : $"{takeNext}&${TAKE_PREV_ARG_NAME}={TakePrevious}";
                return AppendSortOrder(url);
            }

            var target = TargetVideo.HasValue ? TargetVideo.Value.ToString() : TargetVideoKey;

            var query = $"{baseQuery}&${TARGET_VIDEO_ARG_NAME}={target}" +
                   $"&${TAKE_NEXT_ARG_NAME}={TakeNext}" +
                   $"&${TAKE_PREV_ARG_NAME}={TakePrevious}";
            return AppendSortOrder(query);
        }

        private string AppendSortOrder(string url)
        {
            return !SortOrder.HasValue ? url : $"{url}&${SORTING_ORDER_ARG_NAME}={(int)SortOrder.Value}";
        }
    }
}
