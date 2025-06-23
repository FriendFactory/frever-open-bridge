using System;
using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class CrewCompetition
    {
        public int TrophyScore { get; set; }
        public DateTime EndDate { get; set; }
        public CrewReward[] Rewards { get; set; }
        public long[] ClaimedRewardIds { get; set; }
        [ProtoNewField(1)] public int WeekNumber { get; set; }
    }
}