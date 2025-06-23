using System.Collections.Generic;
using Bridge.Models.ClientServer.Assets;

namespace Bridge.Services.UserProfile
{
    public sealed class UpdateProfileRequest
    {
        public readonly long? MainCharacterId;

        public readonly string Nickname;

        public readonly long? Gender;

        public readonly long? CountryId;

        public readonly CharacterAccess? CharacterAccess;
        
        public string Bio { get; set; }

        public Dictionary<string, string> BioLinks { get; set; }
        
        public long UniverseId { get; set; }

        public UpdateProfileRequest(long? mainCharacterId, string nickname, long? gender, long? countryId, CharacterAccess? characterAccess, string bio, Dictionary<string, string> bioLinks)
        {
            MainCharacterId = mainCharacterId;
            Nickname = nickname;
            Gender = gender;
            CountryId = countryId;
            CharacterAccess = characterAccess;
            Bio = bio;
            BioLinks = bioLinks;
        }
    }
}