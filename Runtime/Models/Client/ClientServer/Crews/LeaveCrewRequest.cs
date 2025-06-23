namespace Bridge.Models.ClientServer.Crews
{
    public sealed class LeaveCrewRequest
    {
        public long CrewId { get; set; }
        public long? NewLeaderGroupId { get; set; }
    }
}