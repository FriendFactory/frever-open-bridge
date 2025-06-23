namespace Bridge.Authorization.Models
{
    public class ValidatePasswordStrengthResult
    {
        public bool Ok { get; set; }
        public bool IsStrong { get; set; }
        public bool IsLongEnough { get; set; }
        public bool IsTooSimple { get; set; }
        public string Error { get; set; }
        public string ErrorCode { get; set; }
    }
}