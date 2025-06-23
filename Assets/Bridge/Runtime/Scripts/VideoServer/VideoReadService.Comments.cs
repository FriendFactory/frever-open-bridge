using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Results;

namespace Bridge.VideoServer
{
    internal sealed partial class VideoReadService
    {
        public async Task<ArrayResult<CommentInfo>> GetVideoComments(
            long videoId,
            int skip,
            int take,
            CancellationToken token
        )
        {
            var query = CreateTakeSkipQuery<CommentInfo>(take, skip);

            var url = new Uri(
                new Uri(Host, UriKind.Absolute),
                new Uri($"video/{videoId}/comment", UriKind.Relative)
            ).ToString();

            url += query.BuildQuery();

            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var response = await req.GetHTTPResponseAsync(token);
            if (!response.IsSuccess)
            {
                var error = response.DataAsText;
                return new ArrayResult<CommentInfo>(error);
            }

            var comments = Serializer.DeserializeProtobuf<CommentInfo[]>(response.Data);
            return new ArrayResult<CommentInfo>(comments);
        }

        public async Task<SingleEntityResult<CommentInfo>> GetVideoComment(long videoId, long commentId, CancellationToken token)
        {
            var url = BuildUrl($"video/{videoId}/comment/by-id/{commentId}");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess)
            {
                return new SingleEntityResult<CommentInfo>(resp.DataAsText);
            }

            var model = Serializer.DeserializeProtobuf<CommentInfo>(resp.Data);
            return new SingleEntityResult<CommentInfo>(model);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoRootComments(long videoId, string key, int takeOlder, int takeNewer, CancellationToken token)
        {
            return GetCommentsList($"video/{videoId}/comment/root?key={key}&takeNewer={takeNewer}&takeOlder={takeOlder}", token);
        }

        public Task<ArrayResult<CommentInfo>> GetVideoThreadComments(long videoId, string rootCommentKey, string replyCommentKey, int takeOlder, int takeNewer, CancellationToken token)
        {
            var endpoint =
                $"video/{videoId}/comment/thread/{rootCommentKey}?takeNewer={takeNewer}&takeOlder={takeOlder}";
            
            if (replyCommentKey != null) endpoint += $"&key={replyCommentKey}";
            
            return GetCommentsList(endpoint, token);
        }
        
        public async Task<SingleObjectResult<VideoCommentAuthorsInfo>> GetWhoCommentedTheVideo(long videoId, CancellationToken token)
        {
            var url = BuildUrl($"video/{videoId}/who-commented");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess)
            {
                return new SingleObjectResult<VideoCommentAuthorsInfo>(resp.DataAsText);
            }

            var model = Serializer.DeserializeProtobuf<VideoCommentAuthorsInfo>(resp.Data);
            return new SingleObjectResult<VideoCommentAuthorsInfo>(model);
        }
        
        private async Task<ArrayResult<CommentInfo>> GetCommentsList(string endPoint, CancellationToken token)
        {
            var url = BuildUrl(endPoint);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(token: token);
            if (!resp.IsSuccess)
            {
                return new ArrayResult<CommentInfo>(resp.DataAsText);
            }

            var model = Serializer.DeserializeProtobuf<CommentInfo[]>(resp.Data);
            return new ArrayResult<CommentInfo>(model);
        }

        private string BuildUrl(string endPoint)
        {
            return Path.Combine(Host, endPoint);
        }
    }
}