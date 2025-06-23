using System.Threading;
using System.Threading.Tasks;
using Bridge.AccountVerification.Models;
using Bridge.Results;

namespace Bridge.AccountVerification
{
    public interface IAccountVerificationService
    {
        Task<Result<CredentialStatus>> GetCredentialsStatus(CancellationToken token);
        Task<Result> AddCredentials(AddCredentialsRequest request);
        Task<Result> SendVerificationCode(VerifyCredentialRequest request);
        Task<Result<VerifyUserResponse>> VerifyUserCredentials(VerifyUserRequest request);
        Task<Result> UpdateUserCredentials(UpdateCredentialsRequest request);
    }
}