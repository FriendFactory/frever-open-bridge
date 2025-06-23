namespace Bridge.AccountVerification.Models
{
    public class AddCredentialsRequest
    {
        public CredentialType Type { get; set; }

        public string AppleIdentityToken { get; set; }
        public string AppleId { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }

        public string Password { get; set; }
        
        public string IdentityToken { get; set; }
        public string GoogleId { get; set; }
    }
}