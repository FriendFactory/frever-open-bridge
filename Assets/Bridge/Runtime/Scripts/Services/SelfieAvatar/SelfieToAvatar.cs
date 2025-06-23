using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bridge.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bridge.Services.SelfieAvatar
{
    internal sealed class SelfieToAvatar
    {
        private const string IP = "3.126.234.21";
        private const string SERVICE_URL = "http://" + IP + "/reroute";
        private const string HEALTH_URL = "http://" + IP + "/health";

        private readonly IRequestHelper _requestHelper;
        private readonly FFEnvironment _environment;

        public SelfieToAvatar(IRequestHelper requestHelper, FFEnvironment environment)
        {
            _requestHelper = requestHelper;
            _environment = environment;
        }

        public async Task<SelfieAvatarResult> UploadUserPhotoForProcessing(byte[] byteImage, string gender, float distance, long groupId)
        {
            try
            {
                return await UploadPhotoAsync(byteImage, gender, distance, groupId);
            }
            catch (Exception e)
            {
                return new SelfieAvatarResult(e.Message);
            }
        }
        
        private async Task<SelfieAvatarResult> UploadPhotoAsync(byte[] byteImage, string gender, float distance, long groupId)
        {
            if (await TestConnection() == false)
            {
                return new SelfieAvatarResult("Selfie ML service is offline");
            }

            using (var client = _requestHelper.CreateClient(false))
            {
                var byteImageContent = new ByteArrayContent(byteImage);
                var byteGenderContent = new ByteArrayContent(Encoding.UTF8.GetBytes(gender));
                var byteDistanceContent = new ByteArrayContent(Encoding.UTF8.GetBytes(distance.ToString()));
                var byteEnvironmentContent = new ByteArrayContent(Encoding.UTF8.GetBytes(_environment.ToString()));
                var byteGroupIdContent = new ByteArrayContent(Encoding.UTF8.GetBytes(groupId.ToString()));
            
                var content = new MultipartFormDataContent
                {
                    {byteImageContent, "image", "image.png"},
                    {byteGenderContent, "gender", "gender.dat"},
                    {byteDistanceContent, "distance", "distance.dat"},
                    {byteEnvironmentContent, "env", "environment.dat"},
                    {byteGroupIdContent, "groupid", "groupId.dat"}
                };
                client.DefaultRequestHeaders.Add("emailVerified", "True");

                var res = await client.PostAsync(SERVICE_URL, content);
                if (res.IsSuccessStatusCode == false)
                {
                    return new SelfieAvatarResult(res.ReasonPhrase);
                }
                var response = await res.Content.ReadAsStringAsync();
                response = response.Replace("“", "\"").Replace("”", "\"");
                var photoProcessingData = JsonConvert.DeserializeObject<JSONSelfie>(response);

                if (photoProcessingData.success)
                {
                    return new SelfieAvatarResult(photoProcessingData);
                }

                return new SelfieAvatarResult(photoProcessingData.request_status);
            }
        }
        
        private async Task<bool> TestConnection() 
        {
            var client = new HttpClient();

            var res = await client.GetAsync(HEALTH_URL);

            if (res.IsSuccessStatusCode == false)
                return false;
        
            var content =  await res.Content.ReadAsStringAsync();
        
            var result = JsonConvert.DeserializeObject<HealthResult>(content, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return result.Success;
        }
    }
}
