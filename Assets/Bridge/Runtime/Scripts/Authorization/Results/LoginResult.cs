using System.Collections.Generic;
using System.Linq;
using Bridge.Results;

namespace Bridge.Authorization.Results
{
    public sealed class LoginResult: Result
    {
        public LoginErrorType? ErrorType;

        private static readonly Dictionary<string, LoginErrorType> _errorResponseKeywords =
            new Dictionary<string, LoginErrorType>()
            {
                {"AuthenticateLogin", LoginErrorType.WrongEmail},
                {"AuthenticatePassword", LoginErrorType.WrongPassword}
            };
        
        internal LoginResult()
        {
        }

        internal LoginResult(string errorMessage) : base(errorMessage)
        {
            var errKey = _errorResponseKeywords.Keys.FirstOrDefault(errorMessage.Contains);
            ErrorType = errKey != null ? _errorResponseKeywords[errKey] : LoginErrorType.Other;
        }
    }
}