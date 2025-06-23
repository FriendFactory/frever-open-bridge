namespace Bridge.VideoServer
{
    internal class InitVideoUploadingResponse
    {
        public readonly string UploadFileUrl;

        
        public bool IsSuccess => !string.IsNullOrEmpty(UploadFileUrl);
        
        public InitVideoUploadingResponse(string uploadFileUrl)
        {
            UploadFileUrl = uploadFileUrl;
        }
    }
}