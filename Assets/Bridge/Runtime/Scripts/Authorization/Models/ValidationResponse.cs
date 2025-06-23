namespace Bridge.Authorization.Models
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public string ErrorCode { get; set; }
        public string ValidationError { get; set; }
        public UserRegistrationErrors UserRegistrationErrors { get; set; }
    }
    
    public class UserRegistrationErrors
    {
        public bool UsernameTaken { get; set; }
        public bool UsernameLengthIncorrect { get; set; }
        public bool UsernameContainsForbiddenSymbols { get; set; }
        public bool UsernameModerationFailed { get; set; }
    }
}
