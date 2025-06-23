using Bridge.Results;

namespace Bridge.Services.AssetService
{
    internal sealed class InitUploadResp: Result
    {
        public string UploadId;
        public string UploadUrl;

        //default constructor is necessary for response deserialization
        public InitUploadResp()
        {
                
        }
            
        public InitUploadResp(string errorMessage) : base(errorMessage)
        {
        }
    }
}