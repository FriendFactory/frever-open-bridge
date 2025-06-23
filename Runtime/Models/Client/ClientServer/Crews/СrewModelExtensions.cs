using System.Linq;

namespace Bridge.Models.ClientServer.Crews
{
    public static class СrewModelExtensions
    {
        
        public static CrewShortInfo ToCrewShortInfo(this CrewModel crewModel)
        {
            return new CrewShortInfo
            {
                Id = crewModel.Id,
                Name = crewModel.Name,
                Description = crewModel.Description,
                Files = crewModel.Files,
                FollowersCount = crewModel.Members.Count(member => member.IsFollower),
                FollowingCount = crewModel.Members.Count(member => member.IsFollowing),
                FriendsCount = crewModel.Members.Count(member => member.IsFriend),
                Members = crewModel.Members.Select(member => member.Group).ToArray(),
                MembersCount = crewModel.MembersCount,
                TotalMembersCount = crewModel.TotalMembersCount,
                IsPublic = crewModel.IsPublic,
                TrophyScore = crewModel.Competition.TrophyScore
            };
        }
    }
}