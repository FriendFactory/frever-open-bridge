using System.Threading;
using System.Threading.Tasks;
using Bridge.AccountVerification;
using Bridge.AccountVerification.Models;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private readonly IAccountVerificationRequestsFactory _requestsFactory = new AccountVerificationRequestsFactory();
        
        public Task<Result<CredentialStatus>> GetCredentialsStatus(CancellationToken token = default)
        {
            return _accountVerificationService.GetCredentialsStatus(token);
        }

        public Task<Result> AddVerificationMethod(VerifiableCredentialType type, string credential, string verificationCode)
        {
            var request = _requestsFactory.CreateAddCredentialsRequest(type, credential, verificationCode);

            return _accountVerificationService.AddCredentials(request);
        }

        public Task<Result> AddVerificationMethod(LinkableCredentialType type, string identityToken, string identityId)
        {
            var request = _requestsFactory.CreateAddCredentialsRequest(type, identityToken, identityId);

            return _accountVerificationService.AddCredentials(request);
        }

        public Task<Result> AddVerificationMethod(string password)
        {
            var request = _requestsFactory.CreateAddCredentialsRequest(password);

            return _accountVerificationService.AddCredentials(request);
        }

        public Task<Result> SendVerificationCode(VerifiableCredentialType type, string credential, bool isNew)
        {
            var request = _requestsFactory.CreateVerifyCredentialRequest(type, credential, isNew);

            return _accountVerificationService.SendVerificationCode(request);
        }

        public Task<Result<VerifyUserResponse>> VerifyCredentials(CredentialType type, string verification)
        {
            var request = _requestsFactory.CreateVerifyUserRequest(type, verification);

            return _accountVerificationService.VerifyUserCredentials(request);
        }

        public Task<Result> ChangeVerificationMethod(VerifiableCredentialType type, string credential, string verificationCode, string verificationToken)
        {
            var request = _requestsFactory.CreateUpdateCredentialsRequest(type, credential, verificationCode, verificationToken);

            return _accountVerificationService.UpdateUserCredentials(request);
        }

        public Task<Result> ChangeVerificationMethod(string password, string verificationToken)
        {
            var request = _requestsFactory.CreateUpdateCredentialsRequest(password, verificationToken);

            return _accountVerificationService.UpdateUserCredentials(request);
        }

        public Task<Result> RemoveVerificationMethod(CredentialType type, string verificationCode, string verificationToken)
        {
            var request = _requestsFactory.CreateRemoveCredentialsRequest(type, verificationCode, verificationToken);

            return _accountVerificationService.UpdateUserCredentials(request);
        }
    }
}