using System.Collections.Generic;
using Bridge.AccountVerification.Models;

namespace Bridge.Authorization.Models
{
    public sealed class AppleAuthCredentials : CredentialsBase
    {
        public string Email { get; set; }
        public string AppleIdentityToken { get; set; }
        public string AppleId { get; set; }

        public override CredentialType CredentialType => CredentialType.AppleId;
        public override string GrantType => "apple_auth_token";
        public override string LoginEndPoint => "/Account/LoginWithApple";
        public override IRequestContentComposer RequestContentComposer => new JsonRequestContentComposer(this);

        public override bool IsValid => !string.IsNullOrWhiteSpace(AppleIdentityToken) 
                                        && !string.IsNullOrWhiteSpace(AppleId);

        public AppleAuthCredentials() { }

        public AppleAuthCredentials(string email, string appleIdentityToken, string appleId)
        {
            Email = email;
            AppleIdentityToken = appleIdentityToken;
            AppleId = appleId;
        }
        
        public override Dictionary<string, string> GetRegistrationCredentials()
        {
            return new Dictionary<string, string>
            {
                {"email", Email},
                {"appleIdentityToken", AppleIdentityToken},
                {"appleId", AppleId}
            };
        }
        
        public override Dictionary<string, string> GetLoginCredentials()
        {
            return new Dictionary<string, string>
            {
                {"email", Email},
                {"appleIdentityToken", AppleIdentityToken},
                {"appleId", AppleId},
            };
        }
    }
}