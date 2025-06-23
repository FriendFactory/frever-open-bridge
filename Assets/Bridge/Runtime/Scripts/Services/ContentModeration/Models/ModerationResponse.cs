namespace Bridge.Services.ContentModeration.Models
{
    internal sealed class ModerationResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool PassedModeration { get; set; }
        public string Reason { get; set; }
    }
}
