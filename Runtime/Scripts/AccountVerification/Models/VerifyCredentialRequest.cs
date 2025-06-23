namespace Bridge.AccountVerification.Models
{
    public class VerifyCredentialRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsNew { get; set; }
    }
}