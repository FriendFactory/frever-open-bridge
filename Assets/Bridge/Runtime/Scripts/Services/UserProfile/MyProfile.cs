using System;
using System.Collections.Generic;
using Bridge.Authorization.Models;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Invitation;
using Bridge.Models.ClientServer.UserActivity;

namespace Bridge.Services.UserProfile
{
    public class MyProfile
    {
        public string Nickname { get; set; }

        public long? MainCharacterId { get; set; }

        public long? TaxationCountryId { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsEmployee { get; set; }

        public bool IsFeatured { get; set; }

        public bool? DataCollectionEnabled { get; set; }

        public DateTime? BirthDate { get; set; }

        public long? GenderId { get; set; }

        public bool AnalyticsEnabled { get; set; }

        public UserBalance UserBalance { get; set; }

        public UserCard LevelingProgress { get; set; }

        public CharacterAccess CharacterAccess { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsStarCreator { get; set; }

        public bool IsStarCreatorCandidate { get; set; }

        public FeatureSettings FeatureSettings { get; set; }

        public bool OnboardingTasksCompleted { get; set; }

        public string Bio { get; set; }

        public Dictionary<string, string> BioLinks { get; set; }

        public StarCreator SupportedStarCreator { get; set; }

        public string DetectedLocationCountry { get; set; }

        public bool MusicEnabled { get; set; }
        
        [ProtoNewField(1)] public bool IsMinor { get; set; }
        [ProtoNewField(2)] public bool AdvertisingTrackingEnabled { get; set; }
        
        [ProtoNewField(3)] public ParentalConsent ParentalConsent { get; set; }

        [ProtoNewField(4)] public bool IsParentAgeValidated { get; set; }
        
        [ProtoNewField(5)] public bool IsInAppPurchaseAllowed { get; set; }
        
        [ProtoNewField(6)] public string EmailRedacted { get; set; }
        
        [ProtoNewField(7)] public bool IsNicknameChangeAllowed { get; set; }
        
        [ProtoNewField(8)] public bool IsOnboardingCompleted { get; set; }
        [ProtoNewField(9)] public InitialAccountBalanceInfo InitialAccountBalance { get; set; }
        [ProtoNewField(10)] public DateTime? UsernameUpdateAvailableOn { get; set; }
        [ProtoNewField(11)] public long[] MainCharacterIds { get; set; }
        [ProtoNewField(12)] public bool HasDefaultUsername { get; set; }
        [ProtoNewField(13)] public bool HasUpdatedCredentials { get; set; }
    }
}