using System.Threading;
using System.Threading.Tasks;
using Bridge.AccountVerification.Models;
using Bridge.Results;

namespace Bridge
{
    public interface IAccountVerificationBridge
    {
        Task<Result<CredentialStatus>> GetCredentialsStatus(CancellationToken token = default);
        
        Task<Result> SendVerificationCode(VerifiableCredentialType type, string credential, bool isNew);
        
        Task<Result> AddVerificationMethod(VerifiableCredentialType type, string credential, string verificationCode);
        Task<Result> AddVerificationMethod(LinkableCredentialType type, string identityToken, string identityId);
        Task<Result> AddVerificationMethod(string password);
        
        Task<Result<VerifyUserResponse>> VerifyCredentials(CredentialType type, string verification);
        
        Task<Result> ChangeVerificationMethod(VerifiableCredentialType type, string credential, string verificationCode, string verificationToken);
        Task<Result> ChangeVerificationMethod(string password, string verificationToken);
        Task<Result> RemoveVerificationMethod(CredentialType type, string verificationCode, string verificationToken);
    }
}