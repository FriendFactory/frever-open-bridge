using System;

namespace Bridge.Authorization.Results
{
    public class UpdateUsernameResult
    {
        public bool Ok { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime? UsernameUpdateAvailableOn { get; set; }
        public UserAccountRegistrationErrors UpdateErrorDetails { get; set; }
    }
    
    public class UserAccountRegistrationErrors
    {
        public bool UsernameTaken { get; set; }
        public bool UsernameLengthIncorrect { get; set; }
        public bool UsernameContainsForbiddenSymbols { get; set; }
        public bool UsernameModerationFailed { get; set; }
    }
}