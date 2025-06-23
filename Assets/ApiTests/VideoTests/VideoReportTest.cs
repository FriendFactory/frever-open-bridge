using System.Linq;
using Bridge.VideoServer;

namespace ApiTests.VideoTests
{
    public class VideoReportTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var reportReasonsResp = await Bridge.GetReportReasons();
            var reportReasons = reportReasonsResp.Models;

            var myVideosResp = await Bridge.GetMyVideoListAsync(null, 30);
            var lastMyVideo = myVideosResp.Models.Last().Id;
            var resp = await Bridge.Report(lastMyVideo, new ReportData()
            {
                Message = "Serhii Test",
                ReasonId = reportReasons.First().Id
            });
        }
    }
}