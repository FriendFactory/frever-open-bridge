
using Bridge.Results;

namespace Bridge.Services.SelfieAvatar
{
    public class SelfieAvatarResult : Result
    {
        public readonly JSONSelfie SelfieJson;

        internal SelfieAvatarResult(string errorMessage) : base(errorMessage)
        {
        }

        internal SelfieAvatarResult(JSONSelfie selfie)
        {
            SelfieJson = selfie;
        }
    }
}
