using Bridge.Results;

namespace Bridge.VideoServer
{
    public sealed class VideoReportResult: Result
    {
        public readonly long IncidentId;

        internal VideoReportResult(long incidentId)
        {
            IncidentId = incidentId;
        }

        internal VideoReportResult(string error) : base(error)
        {
            
        }
    }
}