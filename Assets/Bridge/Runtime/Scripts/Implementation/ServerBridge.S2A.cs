using System.Threading.Tasks;
using Bridge.Services.SelfieAvatar;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<SelfieAvatarResult> GetSelfieJsonAsync(byte[] byteImage, string gender, float distance)
        {
            return _selfieToAvatar.UploadUserPhotoForProcessing(byteImage, gender, distance, Profile.GroupId);
        }
    }
}