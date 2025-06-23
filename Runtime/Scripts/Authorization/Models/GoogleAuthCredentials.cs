using System.Collections.Generic;
using Bridge.AccountVerification.Models;

namespace Bridge.Authorization.Models
{
    public sealed class GoogleAuthCredentials : CredentialsBase
    {
        public string Email { get; set; }
        public string GoogleIdentityToken { get; set; }
        public string GoogleId { get; set; }

        public override CredentialType CredentialType => CredentialType.GoogleId;
        public override string GrantType => "google_auth_token";
        public override string LoginEndPoint => "/Account/LoginWithGoogle";
        public override IRequestContentComposer RequestContentComposer => new JsonRequestContentComposer(this);

        public override bool IsValid => !string.IsNullOrWhiteSpace(GoogleIdentityToken) 
                                        && !string.IsNullOrWhiteSpace(GoogleId);
        public GoogleAuthCredentials() { }
        public GoogleAuthCredentials(string email, string googleIdentityToken, string googleId)
        {
            Email = email;
            GoogleIdentityToken = googleIdentityToken;
            GoogleId = googleId;
        }
        
        public override Dictionary<string, string> GetRegistrationCredentials()
        {
            return new Dictionary<string, string>
            {
                {"email", Email},
                {"IdentityToken", GoogleIdentityToken},
                {"GoogleId", GoogleId}
            };
        }
        
        public override Dictionary<string, string> GetLoginCredentials()
        {
            return new Dictionary<string, string>
            {
                {"email", Email},
                {"IdentityToken", GoogleIdentityToken},
                {"GoogleId", GoogleId},
            };
        }
    }
}