using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bridge.Models.ClientServer.Recommendations
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RecommendationReason
    {
        CommonFriends = 1,
        Influential = 2,
        Personalized = 3,
        FollowBack = 4,
    }
}