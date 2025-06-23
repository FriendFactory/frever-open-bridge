using Bridge.Models.ClientServer.Assets;

namespace Bridge.Services.UserProfile
{
    public class PublicProfile
    {
        public long MainGroupId { get; set; }
        public string NickName { get; set; }
        public int FollowersCount { get; set; }
        public CharacterInfo MainCharacter { get; set; }
    }
}