using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using Bridge.Authorization.Results;
using Bridge.Results;
using Models.Client;

namespace Bridge.Authorization
{
    internal interface IAuthService : IHostProvider
    {
        FFEnvironment Environment { get; }
        AuthToken Token { get; }
        UserProfile Profile { get; }
        bool IsLoggedIn { get; }
        Task<LoginResult> LogInAsync(ICredentials credentials);
        Task<LoginResult> LogInAsync(AuthToken token);
        Task<Result> RequestPhoneNumberVerificationCode(string phoneNumber);
        Task<Result> RequestEmailVerification(string email);
        Task<Result> VerifyMyParentEmail(string verificationCode);
        Task<Result> RequestMyParentEmailVerification();
        Task<RegistrationResult> RegisterAsync(UserRegistrationModel userData);
        LogOutResult LogOut();
        Task<CredentialValidationResult> ValidateRegistrationCredentials(ValidationModel validationModel);
        Task<CredentialValidationResult> ValidateLoginCredentials(ValidationModel validationModel);
        Task<ValidationInvitationCodeResult> ValidateInvitationCode(string invitationCode);
        Task<Result<string[]>> GetSuggestedNicknames(int count, CancellationToken token);
        Task<Result> StoreEmailForAppleId(string appleId, string email);
        Task<Result<ServicesUrls>> GetServicesUrls(CancellationToken token = default);
        Task<Result> RequestParentEmailVerificationCode(string parentEmail);
        Task<AssignParentEmailResult> AssignParentEmail(string parentEmail, string verificationCode);
        Task<Result<bool>> CheckIfParentEmailBound(string username);
        Task<Result> ConfigureParentalConsent(ConfigureParentalConsentRequest configureParentalConsentRequest);
        Task<Result> RegisterTemporaryAccount(TemporaryAccountRequestModel model);
        Task<Result<ValidatePasswordStrengthResult>> ValidatePasswordStrength(string password, string nickname);
        Task<UpdateUsernameResult> UpdateUsername(string username);
        Task<Result> UpdateUserInfo(UpdateUserModel model);
    }

    internal interface IHostProvider
    {
        string AuthUrl { get; }
        string ClientServerHost { get; }
        string MainServerHost { get; }
        string AssetManagerServerHost { get; }
        string AssetSeverHost { get; }
        string UserServerHost { get; }
        string VideoServerHost { get; }
        string NotificationServerHost { get; }
        string ChatServerHost { get; }
    }
}
