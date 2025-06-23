namespace Bridge.Models.ClientServer.Recommendations
{
    public class FollowRecommendation
    {
        public RecommendationGroupInfo Group { get; set; }
        public RecommendationReason Reason { get; set; }
        public RecommendationGroupInfo[] CommonFriends { get; set; }
    }
}
