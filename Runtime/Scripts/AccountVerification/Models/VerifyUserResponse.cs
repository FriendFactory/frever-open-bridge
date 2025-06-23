namespace Bridge.AccountVerification.Models
{
    public class VerifyUserResponse
    {
        public bool IsSuccessful { get; set; }
        public string VerificationToken { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}