using Bridge.Results;

namespace Bridge.Services.UserProfile
{
    public sealed class ProfileResult<T>: Result
    {
        public readonly T Profile;

        public ProfileResult(T profile)
        {
            Profile = profile;
        }

        public ProfileResult(string errorMessage, int? statusCode = null) : base(errorMessage, statusCode)
        {
        }

        public static ProfileResult<T> CanceledInstance()
        {
            return new ProfileResult<T>();
        }

        private ProfileResult() : base(true)
        {
            //canceled result
        }
    }
}