using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bridge.AccountVerification.Models;

namespace Bridge.Authorization.Models
{
    [Serializable]
    public sealed class PhoneNumberCredentials : CredentialsBase
    {
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }

        private static readonly Regex PhoneNumberRegex = new Regex(@"\+\d{10,15}");

        public override bool IsValid => !string.IsNullOrWhiteSpace(VerificationCode) && PhoneNumberRegex.IsMatch(PhoneNumber);
        
        public PhoneNumberCredentials() { }

        public PhoneNumberCredentials(string phoneNumber, string verificationCode)
        {
            PhoneNumber = phoneNumber;
            VerificationCode = verificationCode;
        }

        public override CredentialType CredentialType => CredentialType.PhoneNumber;
        public override string GrantType => "phone_number_token";
        public override string LoginEndPoint => "connect/token";
        public override IRequestContentComposer RequestContentComposer => new MultipartRequestContentComposer(this);

        public override Dictionary<string, string> GetRegistrationCredentials()
        {
            return new Dictionary<string, string>
            {
                {"phoneNumber", PhoneNumber},
                {"verificationCode", VerificationCode}
            };
        }
        
        public override Dictionary<string, string> GetLoginCredentials()
        {
            return new Dictionary<string, string>
            {
                {"scope", "friends_factory.creators_api offline_access" },
                {"grant_type", GrantType},
                {"phone_number", PhoneNumber},
                {"verification_token", VerificationCode},
            };
        }
    }
}