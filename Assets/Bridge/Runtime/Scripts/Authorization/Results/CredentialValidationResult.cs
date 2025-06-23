using Bridge.Authorization.Models;
using Bridge.Results;

namespace Bridge.Authorization.Results
{
    public sealed class CredentialValidationResult: Result
    {
        public readonly ValidationResponse Data;

        internal CredentialValidationResult(ValidationResponse data)
        {
            Data = data;
        }

        internal CredentialValidationResult(string errorMessage): base(errorMessage)
        {
            
        }
    }
}
