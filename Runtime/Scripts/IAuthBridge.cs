using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Authorization.Models;
using Bridge.Authorization.Results;
using Bridge.EnvironmentCompatibility;
using Bridge.Results;

namespace Bridge
{
    public interface IAuthBridge
    {
        AuthToken Token { get; }
        UserProfile Profile { get; }
        bool RememberLastUser { get; }
        bool IsLoggedIn { get; }
        Task<EnvironmentCompatibilityResult> GetEnvironmentCompatibilityData(FFEnvironment environment);
        
        Task<Result> StoreEmailForAppleId(string appleId, string email);
        Task<Result> RequestPhoneNumberVerificationCode(string phoneNumber);
        Task<Result> RequestEmailVerificationCode(string email);
        Task<Result> RequestMyParentEmailVerificationCode();
        Task<Result> VerifyMyParentEmail(string verificationCode);
        Task<LoginResult> LogInAsync(ICredentials credentials, bool savePassword);
        Task<LoginResult> LoginToLastSavedUserAsync();
        /// <summary>
        /// Used with 'user' console command only
        /// </summary>
        Task<LoginResult> ReloadLastSavedUserAndLogin();
        Task<CredentialValidationResult> ValidateRegistrationCredentials(ValidationModel validationModel);
        Task<CredentialValidationResult> ValidateLoginCredentials(ValidationModel validationModel);
        Task<Result<string[]>> GetSuggestedNicknames(int count, CancellationToken token = default);
        Task<ValidationInvitationCodeResult> ValidateInvitationCode(string invitationCode);
        Task<RegistrationResult> RegisterAsync(UserRegistrationModel userData, bool savePassword);
        Task<Result> ConfigureParentalConsent(ConfigureParentalConsentRequest configureParentalConsentRequest);
        Task<Result> RequestParentEmailVerificationCode(string parentEmail);
        Task<AssignParentEmailResult> AssignParentEmail(string parentEmail, string verificationCode);
        LogOutResult LogOut(bool clearCache = false);
        Task<Result<bool>> CheckIfParentEmailBound(string username);
        Task<Result> RegisterTemporaryAccount(TemporaryAccountRequestModel model, bool saveToken = true);
        void SaveToken();
        Task<Result<ValidatePasswordStrengthResult>> ValidatePasswordStrength(string password, string nickname);
        Task<UpdateUsernameResult> UpdateUsername(string username);
        Task<Result> UpdateUserInfo(UpdateUserModel model);
    }
}