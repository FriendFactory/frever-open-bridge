using System.Threading.Tasks;
using Bridge.Services.SelfieAvatar;

namespace Bridge
{
    public interface IS2ABridge
    {
        Task<SelfieAvatarResult> GetSelfieJsonAsync(byte[] byteImage, string gender, float distance);
    }
}