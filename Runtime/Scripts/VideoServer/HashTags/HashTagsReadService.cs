using System;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.VideoServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.VideoServer.HashTags
{
    internal sealed class HashTagsReadService
    {
        private readonly string _hostUrl;
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;

        public HashTagsReadService(string hostUrl, IRequestHelper requestHelper, ISerializer serializer)
        {
            _hostUrl = hostUrl;
            _requestHelper = requestHelper;
            _serializer = serializer;
        }

        public async Task<ArrayResult<HashtagInfo>> GetHashtags(string filter, int skip, int take, CancellationToken cancellationToken)
        {
            try
            {
                return await GetHashTagsInternal(filter, skip, take, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<HashtagInfo>();
            }
        }

        private async Task<ArrayResult<HashtagInfo>> GetHashTagsInternal(string filter, int skip, int take, CancellationToken cancellationToken)
        {
            var url = Extensions.CombineUrls(_hostUrl, $"hashtag/all?name={filter}&skip={skip}&take={take}");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(token: cancellationToken);
            if (!resp.IsSuccess)
            {
                return new ArrayResult<HashtagInfo>(resp.DataAsText);
            }

            var hashtags = _serializer.DeserializeProtobuf<HashtagInfo[]>(resp.Data);
            return new ArrayResult<HashtagInfo>(hashtags);
        }
    }
}
