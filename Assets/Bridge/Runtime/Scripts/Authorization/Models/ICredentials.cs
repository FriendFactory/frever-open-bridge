using System.Collections.Generic;
using Bridge.AccountVerification.Models;

namespace Bridge.Authorization.Models
{
    public interface ICredentials
    {
        CredentialType CredentialType { get; }
        string GrantType { get; }
        
        string LoginEndPoint { get; }

        IRequestContentComposer RequestContentComposer { get; }
        
        Dictionary<string, string> GetRegistrationCredentials();
        Dictionary<string, string> GetLoginCredentials();

        bool IsValid { get; }
    }

    public abstract class CredentialsBase : ICredentials
    {
        public abstract CredentialType CredentialType { get; }

        /// <summary>
        /// Necessary only for registration
        /// </summary>
        public abstract string GrantType { get; }
        public abstract string LoginEndPoint { get; }
        public abstract IRequestContentComposer RequestContentComposer { get; }
        public abstract Dictionary<string, string> GetRegistrationCredentials();
        public abstract Dictionary<string, string> GetLoginCredentials();
        public abstract bool IsValid { get; }
    }
}