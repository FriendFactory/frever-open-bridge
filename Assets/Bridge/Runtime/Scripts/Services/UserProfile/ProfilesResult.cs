using Bridge.Results;

namespace Bridge.Services.UserProfile
{
    public sealed class ProfilesResult<T>: Result
    {
        public readonly T[] Profiles;

        public ProfilesResult(T[] profiles)
        {
            Profiles = profiles;
        }

        public ProfilesResult(string errorMessage, int? statusCode = null) : base(errorMessage, statusCode)
        {
        }

        internal static ProfilesResult<T> CanceledInstance()
        {
            return new ProfilesResult<T>();
        }
        
        private ProfilesResult():base(true)
        {
            //canceled result
        }
    }
}