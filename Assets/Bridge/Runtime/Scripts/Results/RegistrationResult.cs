using System.Collections.Generic;
using System.Linq;

namespace Bridge.Results
{
    public sealed class RegistrationResult:Result
    {
        public RegistrationErrorType? ErrorType;

        private static readonly Dictionary<string, RegistrationErrorType> _errorResponseKeywords =
            new Dictionary<string, RegistrationErrorType>()
            {
                {"already exists", RegistrationErrorType.UserAlreadyExists},
            };
        
        internal RegistrationResult()
        {
        }

        internal RegistrationResult(string errorMessage) : base(errorMessage)
        {
            var errKey = _errorResponseKeywords.Keys.FirstOrDefault(errorMessage.Contains);
            ErrorType = errKey != null ? _errorResponseKeywords[errKey] : RegistrationErrorType.Other;
        }
    }
}