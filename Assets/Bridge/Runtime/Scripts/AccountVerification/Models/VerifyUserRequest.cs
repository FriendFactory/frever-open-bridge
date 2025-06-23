namespace Bridge.AccountVerification.Models
{
    public class VerifyUserRequest
    {
        public CredentialType Type { get; set; }
        public string AppleIdentityToken { get; set; }
        public string IdentityToken { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }
    }
}