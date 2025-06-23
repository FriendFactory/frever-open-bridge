using System.IO;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Results;
using Newtonsoft.Json;

namespace Bridge.VideoServer
{
    internal sealed class VideoReportService : IVideoReportService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly string _serverUrl;

        internal VideoReportService(string serverUrl, IRequestHelper requestHelper)
        {
            _serverUrl = serverUrl;
            _requestHelper = requestHelper;
        }

        public async Task<VideoReportResult> Report(long videoId, ReportData reportData)
        {
            var url = Path.Combine(_serverUrl, $"video/{videoId}/report").FixUrlSlashes();
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var json = JsonConvert.SerializeObject(reportData);
            req.AddJsonContent(json);

            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new VideoReportResult(resp.DataAsText);

            var respJson = resp.DataAsText;
            var respModel = JsonConvert.DeserializeObject<ReportVideoResponse>(respJson);
            return new VideoReportResult(respModel.IncidentId);
        }

        public async Task<ArrayResult<VideoReportReason>> GetReportReasons()
        {
            var url = Path.Combine(_serverUrl, "video-report/moderation/reasons").FixUrlSlashes();
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new ArrayResult<VideoReportReason>(resp.DataAsText);

            var json = resp.DataAsText;
            var reasons = JsonConvert.DeserializeObject<VideoReportReason[]>(json);
            return new ArrayResult<VideoReportReason>(reasons);
        }

        private class ReportVideoResponse
        {
            public long IncidentId { get; set; }
        }
    }
}