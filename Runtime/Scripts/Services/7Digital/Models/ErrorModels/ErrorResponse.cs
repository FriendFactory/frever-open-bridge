namespace Bridge.Services._7Digital.Models
{
    internal sealed class ErrorResponse 
    {
        public string status { get; set; }
        public string version { get; set; }
        public Error Error { get; set; }
    }
}
