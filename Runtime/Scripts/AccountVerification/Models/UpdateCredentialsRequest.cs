namespace Bridge.AccountVerification.Models
{
    public class UpdateCredentialsRequest
    {
        public CredentialType Type { get; set; }
        public string VerificationToken { get; set; }
        public string VerificationCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    } 
}