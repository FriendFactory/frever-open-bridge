using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bridge.AccountVerification.Models;

namespace Bridge.Authorization.Models
{
    public abstract class EmailCredentialsBase : CredentialsBase
    {
        public string Email { get; set; }

        public sealed override string LoginEndPoint => "connect/token";
        public override IRequestContentComposer RequestContentComposer => new MultipartRequestContentComposer(this);
        
        private static readonly Regex EMAIL_REGEX = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        
        public bool IsEmailValid()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            var emailMatch = EMAIL_REGEX.Match(Email);
            return emailMatch.Success;
        }
    }
    
    [Serializable]
    public sealed class EmailCredentials : EmailCredentialsBase
    {
        private const int VERIFICATION_CODE_LENGTH = 6;
        
        public string VerificationCode { get; set; }

        private static readonly Regex VER_CODE_REGEX = new Regex($"^[0-9]+$");
        
        public override bool IsValid =>  IsEmailValid() && IsVerificationCodeValid();

        public override CredentialType CredentialType => CredentialType.Email;
        public override string GrantType => "email_auth_token";
        public override IRequestContentComposer RequestContentComposer => new MultipartRequestContentComposer(this);

        public bool IsVerificationCodeValid()
        {
            return !string.IsNullOrEmpty(VerificationCode) 
                   && VerificationCode.Length == VERIFICATION_CODE_LENGTH
                   && VER_CODE_REGEX.IsMatch(VerificationCode);
        }

        public override Dictionary<string, string> GetRegistrationCredentials()
        {
            return new Dictionary<string, string>
            {
                {"email", Email},
                {"verificationCode", VerificationCode}
            };
        }
        
        public override Dictionary<string, string> GetLoginCredentials()
        {
            return new Dictionary<string, string>
            {
                {"scope", "friends_factory.creators_api offline_access" },
                {"grant_type", GrantType},
                {"email", Email},
                {"verification_token", VerificationCode}
            };
        }
    }

    [Serializable]
    public sealed class EmailAndPasswordCredentials : EmailCredentialsBase
    {
        public string Password { get; set; }
        public override bool IsValid => IsEmailValid() && !string.IsNullOrEmpty(Password);

        public override CredentialType CredentialType => CredentialType.Password;
        public override string GrantType => "password";

        public override Dictionary<string, string> GetRegistrationCredentials()
        {
            throw new Exception("Registration with email and password is obsolete");
        }

        public override Dictionary<string, string> GetLoginCredentials()
        {
            return new Dictionary<string, string>
            {
                {"scope", "friends_factory.creators_api offline_access" },
                {"grant_type", GrantType},
                {"email", Email},
                {"username", Email},
                {"password", Password}
            };
        }
    }
    
    [Serializable]
    public sealed class UsernameAndPasswordCredentials : EmailCredentialsBase
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public override bool IsValid => !string.IsNullOrEmpty(Password);

        public override CredentialType CredentialType => CredentialType.Password;
        public override string GrantType => "password";

        public override Dictionary<string, string> GetRegistrationCredentials()
        {
            return new Dictionary<string, string>
            {
                {"username", Username},
                {"password", Password},
            };
        }

        public override Dictionary<string, string> GetLoginCredentials()
        {
            return new Dictionary<string, string>
            {
                {"scope", "friends_factory.creators_api offline_access" },
                {"grant_type", GrantType},
                {"username", Username},
                {"password", Password}
            };
        }
    }
}