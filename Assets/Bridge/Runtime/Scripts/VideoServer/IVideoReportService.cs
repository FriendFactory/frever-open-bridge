using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.VideoServer
{
    internal interface IVideoReportService
    {
        Task<VideoReportResult> Report(long videoId, ReportData reportData);

        Task<ArrayResult<VideoReportReason>> GetReportReasons();
    }
}