using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Authorization.Models;
using Bridge.Authorization.Results;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public async Task<LoginResult> LogInAsync(ICredentials credentials, bool savePassword)
        {
            SetupAuthBridge(Environment);

            var res = await _authService.LogInAsync(credentials);
            
            if (!res.IsError)
            {
                OnUserAuthorized(savePassword);
            }

            return res;
        }

        public Task<Result> StoreEmailForAppleId(string appleId, string email)
        {
            SetupAuthBridge(Environment);
            return _authService.StoreEmailForAppleId(appleId, email);
        }

        public Task<Result> RequestPhoneNumberVerificationCode(string phoneNumber)
        {
            SetupAuthBridge(Environment);
            return _authService.RequestPhoneNumberVerificationCode(phoneNumber);
        }
        
        public Task<Result> RequestEmailVerificationCode(string email)
        {
            SetupAuthBridge(Environment);
            return _authService.RequestEmailVerification(email);
        }

        public Task<Result> RequestMyParentEmailVerificationCode()
        {
            return _authService.RequestMyParentEmailVerification();
        }

        public Task<Result> VerifyMyParentEmail(string verificationCode)
        {
            return _authService.VerifyMyParentEmail(verificationCode);
        }

        /// <summary>
        /// Used with 'user' console command only
        /// </summary>
        public Task<LoginResult> ReloadLastSavedUserAndLogin()
        {
            _userDataStorage.Load();
            return LoginToLastSavedUserAsync();
        }
        
        public async Task<LoginResult> LoginToLastSavedUserAsync()
        {
            if (!_userDataStorage.HasSavedData)
            {
                return new LoginResult("No saved info");
            }

            Environment = _userDataStorage.UserData.FfEnvironment;
            SetupAuthBridge(Environment);

            var token = _userDataStorage.UserData.Token;
            var res = await _authService.LogInAsync(token);
            if (!res.IsError)
            {
                OnUserAuthorized(true);
            }

            return res;
        }

        public Task<CredentialValidationResult> ValidateRegistrationCredentials(ValidationModel validationModel)
        {
            SetupAuthBridge(Environment);
            return _authService.ValidateRegistrationCredentials(validationModel);
        }

        public Task<CredentialValidationResult> ValidateLoginCredentials(ValidationModel validationModel)
        {
            SetupAuthBridge(Environment);
            return _authService.ValidateLoginCredentials(validationModel);
        }

        public Task<ValidationInvitationCodeResult> ValidateInvitationCode(string invitationCode)
        {
            SetupAuthBridge(Environment);
            return _authService.ValidateInvitationCode(invitationCode);
        }

        public Task<Result<string[]>> GetSuggestedNicknames(int count, CancellationToken token = default)
        {
            SetupAuthBridge(Environment);
            return _authService.GetSuggestedNicknames(count, token);
        }

        public async Task<RegistrationResult> RegisterAsync(UserRegistrationModel userData, bool savePassword)
        {
            SetupAuthBridge(Environment);

            var res = await _authService.RegisterAsync(userData);

            if (res.IsError) return res;

            OnUserAuthorized(savePassword);
            return res;
        }

        private void SetupAuthBridge(FFEnvironment ffEnvironment)
        {
            if (_authService != null && _authService.Environment == ffEnvironment)
                return;
            var config = _authServerConfigurations.GetConfig(ffEnvironment);
            _authService = new AuthService(_requestHelper, _serializer,  ApiVersion, config);
            Environment = ffEnvironment;
        }
        
        public LogOutResult LogOut(bool clearCache = false)
        {
            if (!IsLoggedIn)
                return new LogOutResult("User is not logged in");

            var res = _authService.LogOut();
            if (res.IsError) return res;
            
            _userDataStorage.Clear();
            SetupNewSession();

            if (clearCache)
            {
                StartCacheClearing();
            }
            else
            {
                _assetsCache = null;
            }
            
            _authService = null;
           
            _savingVersionFilesTrigger?.Stop();
            _savingVersionFilesTrigger = null;

            return res;

            async void StartCacheClearing()
            {
                await ClearCacheAsync(false, Environment);
                _assetsCache = null;
            }
        }

        public Task<Result<bool>> CheckIfParentEmailBound(string username)
        {
            SetupAuthBridge(Environment);
            return _authService.CheckIfParentEmailBound(username);
        }

        public Task<Result> ConfigureParentalConsent(ConfigureParentalConsentRequest configureParentalConsentRequest)
        {
            return _authService.ConfigureParentalConsent(configureParentalConsentRequest);
        }

        public Task<Result> RequestParentEmailVerificationCode(string parentEmail)
        {
            return _authService.RequestParentEmailVerificationCode(parentEmail);
        }

        public Task<AssignParentEmailResult> AssignParentEmail(string parentEmail, string verificationCode)
        {
            return _authService.AssignParentEmail(parentEmail, verificationCode);
        }

        public async Task<Result> RegisterTemporaryAccount(TemporaryAccountRequestModel model, bool saveToken = true)
        {
            SetupAuthBridge(Environment);
            var result = await _authService.RegisterTemporaryAccount(model);
            if (result.IsSuccess) OnUserAuthorized(saveToken);

            return result;
        }

        public void SaveToken()
        {
            if (Token == null) return;
            SaveUserData();
        }

        public Task<Result<ValidatePasswordStrengthResult>> ValidatePasswordStrength(string password, string nickname)
        {
            SetupAuthBridge(Environment);
            return _authService.ValidatePasswordStrength(password, nickname);
        }

        public Task<UpdateUsernameResult> UpdateUsername(string username)
        {
            return _authService.UpdateUsername(username);
        }

        public Task<Result> UpdateUserInfo(UpdateUserModel model)
        {
            return _authService.UpdateUserInfo(model);
        }
    }
}