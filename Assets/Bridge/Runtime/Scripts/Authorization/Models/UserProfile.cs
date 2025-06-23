using System;

namespace Bridge.Authorization.Models
{
    [Serializable]
    public sealed class UserProfile
    {
        public long Id { get; internal set; }
        public string Email { get; internal set; }
        public long GroupId { get; internal set; }
        public bool IsQA { get; internal set; }
        public bool IsEmployee { get; internal set; }
        public bool IsModerator { get; internal set; }
        public bool IsArtist { get; internal set; }
        public bool RegisteredWithAppleId { get; internal set; }
        
        /// <summary>
        /// MainCharacterId at login time moment
        /// </summary>
        public long? MainCharacterId { get; internal set; } 
       
        /// <summary>
        /// IsOnboardingCompleted at login time moment
        /// </summary>
        public bool IsOnboardingCompleted { get; internal set; }
    }
}
