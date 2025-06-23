using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Crews;

namespace Bridge.Services.UserProfile
{
    public class Profile
    {
        public long MainGroupId { get; set; }

        public string NickName { get; set; }

        public int CreatorScoreBadge { get; set; }

        public ProfileKPI KPI { get; set; } = new ProfileKPI();

        public CharacterInfo MainCharacter { get; set; }

        public bool YouFollowUser { get; set; }

        public bool UserFollowsYou { get; set; }

        public CharacterAccess CharacterAccess { get; set; }
        
        public string Bio { get; set; }

        public Dictionary<string, string> BioLinks { get; set; }
        
        public CrewProfile CrewProfile { get; set; }
        
        [ProtoNewField(1)] public bool IsMinor { get; set; }

        [ProtoNewField(2)] public bool AllowChat { get; set; }

        [ProtoNewField(3)] public bool IsNewFriend { get; set; }

        [ProtoNewField(4)] public DateTime ChatAvailableAfterTime { get; set; }
    }
}