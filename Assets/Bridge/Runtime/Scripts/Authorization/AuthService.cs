using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization.Configs;
using Bridge.Authorization.Models;
using Bridge.Authorization.Results;
using Bridge.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;
using JetBrains.Annotations;
using Models.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Bridge.Authorization
{
    internal sealed class AuthService : ServiceBase, IAuthService
    {
        private const string ClientIdAndSecret =
            @"Server:g'KpB#[_(<KP;Et25]a__J*mh;?(=h%NPd('=Z9KpC2xQ.F-E5{Sd>*AtK!t!t9-_Zq9-{.r";

        private const string DEVICE_ID_HEADER = "X-Device-Id";
        private static string DeviceId => SystemInfo.deviceUniqueIdentifier;
        
        private string ApiVersion { get; }
        public string AuthUrl => Config.IdentityServerURL.AddApiVersion(ApiVersion);

        public AuthService(IRequestHelper requestHelper, ISerializer serializer, string apiVersion, AuthConfig config) : base(requestHelper, serializer)
        {
            ApiVersion = apiVersion;
            Config = config;
        }

        public FFEnvironment Environment => Config.Environment;
        public AuthConfig Config { get; }
        public UserProfile Profile { get; } = new();

        public bool IsLoggedIn
        {
            get => Token != null;
            private set => Token = null;
        }

        protected override string Host => Config.IdentityServerURL;

        public string ClientServerHost { get; private set; }
        public string MainServerHost { get; private set; }
        public string AssetManagerServerHost { get; private set; }
        public string AssetSeverHost { get; private set; }
        public string VideoServerHost { get; private set; }
        public string UserServerHost { get; private set; }
        public string NotificationServerHost { get; private set; }
        public string ChatServerHost { get; private set; }
        private AuthToken _token;

        public AuthToken Token
        {
            get => _token;
            set
            {
                _token = value;
                RequestHelper.SetToken(value);
            }
        }

        public async Task<LoginResult> LogInAsync(ICredentials credentials)
        {
            if (!credentials.IsValid) return new LoginResult("Credentials is not valid");

            var url = UrlCombine(AuthUrl, credentials.LoginEndPoint);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            var byteArray = new UTF8Encoding().GetBytes(
                @"Server:g'KpB#[_(<KP;Et25]a__J*mh;?(=h%NPd('=Z9KpC2xQ.F-E5{Sd>*AtK!t!t9-_Zq9-{.r"
            );

            req.AddHeader("Authorization", $"Basic {Convert.ToBase64String(byteArray)}");
            AddDeviceIdHeader(req);
            credentials.RequestContentComposer.ComposeRequestContent(req);

            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                var error = resp.DataAsText;
                return new LoginResult(error);
            }

            var respJson = resp.DataAsText;
            var authResp = Serializer.DeserializeJson<TokenResponse>(respJson);
            SetData(authResp);

            return new LoginResult();
        }

        public async Task<LoginResult> LogInAsync(AuthToken token)
        {
            Token = token;
            if (Token == null) return new LoginResult("Token null");

            try
            {
                return await LoginAsync();
            }
            catch (Exception e)
            {
                IsLoggedIn = false;
                return new LoginResult(e.Message);
            }
        }

        public async Task<RegistrationResult> RegisterAsync(UserRegistrationModel userData)
        {
            var credentials = userData.Credentials;
            if (!credentials.IsValid) return new RegistrationResult("Credentials are not valid");

            var url = Extensions.CombineUrls(AuthUrl, "/Account/Register");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            var secretIdBytes = new UTF8Encoding().GetBytes(ClientIdAndSecret);
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(secretIdBytes)}");
            request.AddHeader("Content-Type", "application/json");
            AddDeviceIdHeader(request);
            var registrationRequestBody = new Dictionary<string, object>
            {
                {"userName", userData.UserName},
                {"birthDate", userData.BirthDate.ToString("yyyy-MM-dd")},
                {"gender", userData.Gender},
                {"allowDataCollection", userData.AllowDataCollection},
                {"analyticsEnabled", userData.AnalyticsEnabled},
                {"country", userData.Country},
                {"defaultLanguage", userData.DefaultLanguage}
            };

            foreach (var keyValuePair in userData.Credentials.GetRegistrationCredentials())
                registrationRequestBody.Add(keyValuePair.Key, keyValuePair.Value);

            var json = Serializer.SerializeToJson(registrationRequestBody);
            request.AddJsonContent(json);

            var resp = await request.GetHTTPResponseAsync();

            if (!resp.IsSuccess)
                return new RegistrationResult($"{resp.DataAsText}");

            var respJson = resp.DataAsText;
            var tokenResponse = Serializer.DeserializeJson<TokenResponse>(respJson);
            SetData(tokenResponse);
            return new RegistrationResult();
        }

        public Task<Result> RequestPhoneNumberVerificationCode(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(phoneNumber));

            var body = new {PhoneNumber = phoneNumber};
            return RequestVerificationCode("/api/verify-phone-number", body);
        }

        public Task<Result> RequestEmailVerification(string email)
        {
            var req = new VerifyEmailRequest
            {
                Email = email
            };
            return RequestVerificationCode("/api/verify-email", req);
        }

        public async Task<Result> RequestMyParentEmailVerification()
        {
            try
            {
                var url = ConcatUrl(AuthUrl, $"/Account/VerifyMyAccountEmail");
                return await SendPostRequest(url);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }
        
        public async Task<Result> VerifyMyParentEmail(string verificationCode)
        {
            try
            {
                var url = ConcatUrl(AuthUrl, "Account/VerifyMyAccountEmailCode");
                var body = new
                {
                    verificationCode
                };
                return await SendPostRequest(url, body);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        private async Task<Result> RequestVerificationCode(string endPoint, object reqBody)
        {
            var url = Extensions.CombineUrls(AuthUrl, endPoint);
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            AddDeviceIdHeader(request);
            var json = Serializer.SerializeToJson(reqBody,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}
            );

            request.AddJsonContent(json);
            var resp = await request.GetHTTPResponseAsync();

            if (!resp.IsSuccess) return new ErrorResult(resp.DataAsText);

            return new SuccessResult();
        }

        public LogOutResult LogOut()
        {
            if (!IsLoggedIn) return new LogOutResult("User is not logged");

            IsLoggedIn = false;

            return new LogOutResult();
        }

        public Task<CredentialValidationResult> ValidateRegistrationCredentials(ValidationModel validationModel)
        {
            return SendValidationCredentialsRequest(validationModel, "ValidateRegistrationInfo");
        }
        
        public Task<CredentialValidationResult> ValidateLoginCredentials(ValidationModel validationModel)
        {
            return SendValidationCredentialsRequest(validationModel, "ValidateLoginInfo");
        }

        public async Task<ValidationInvitationCodeResult> ValidateInvitationCode(string invitationCode)
        {
            var url = UrlCombine(AuthUrl, "invitation-code/validate");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            AddDeviceIdHeader(req);
            var json = Serializer.SerializeToJson(new
            {
                Code = invitationCode
            });
            req.AddJsonContent(json);
            
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return ValidationInvitationCodeResult.Error(resp.Message);

            var respModel = Serializer.DeserializeJson<ValidationInvitationCodeResp>(resp.DataAsText);
            return respModel.IsValid ? ValidationInvitationCodeResult.Valid() : ValidationInvitationCodeResult.NonValid(respModel.Error);
        }

        public Task<Result<string[]>> GetSuggestedNicknames(int count, CancellationToken token)
        {
            try
            {
                var url = UrlCombine(AuthUrl, $"account/suggestnicknames?count={count}");
                return SendPostRequest<string[]>(url, token);
            }
            catch (Exception e)
            {
                return Task.FromResult(e is OperationCanceledException
                    ? Result<string[]>.Cancelled()
                    : Result<string[]>.Error(e.Message));
            }
        }

        public async Task<Result> StoreEmailForAppleId(string appleId, string email)
        {
            var url = UrlCombine(AuthUrl, "/Account/StoreEmailForAppleId");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            var body = new
            {
                AppleId = appleId,
                Email = email
            };
            req.AddJsonContent(Serializer.SerializeToJson(body));
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
            {
                return new SuccessResult();
            }
            return new ErrorResult(resp.DataAsText);
        }

        public async Task<Result<ServicesUrls>> GetServicesUrls(CancellationToken token = default)
        {
            try
            {
                var url = UrlCombine(AuthUrl, "api/client/urls");
                var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, false, false);
                AddDeviceIdHeader(req);
                var resp = await req.GetHTTPResponseAsync(token);
                if (!resp.IsSuccess)
                {
                    return Result<ServicesUrls>.Error(resp.Message);
                }
                
                var responseModel = Serializer.DeserializeJson<ServicesUrls>(resp.DataAsText);
                return Result<ServicesUrls>.Success(responseModel);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    return Result<ServicesUrls>.Cancelled();
                }
                return Result<ServicesUrls>.Error(e.Message);
            }
        }

        private async Task<CredentialValidationResult> SendValidationCredentialsRequest(ValidationModel validationModel, string validationEndPoint)
        {
            var url = UrlCombine(AuthUrl, $"Account/{validationEndPoint}");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            AddDeviceIdHeader(request);
            var json = Serializer.SerializeToJson(validationModel);
            request.AddJsonContent(json);
            var response = await request.GetHTTPResponseAsync();
            if (!response.IsSuccess)
                return new CredentialValidationResult(response.Message);

            var responseJson = response.DataAsText;
            var responseModel = Serializer.DeserializeJson<ValidationResponse>(responseJson);
            return new CredentialValidationResult(responseModel);
        }

        public async Task<Result> RequestParentEmailVerificationCode(string parentEmail)
        {
            try
            {
                var url =  UrlCombine(AuthUrl, "account/verifyparentemail");
                var body = new
                {
                    parentEmail
                };
                return await SendPostRequest(url, body);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<AssignParentEmailResult> AssignParentEmail(string parentEmail, string verificationCode)
        {
            try
            {
                var url = UrlCombine(AuthUrl, "account/assignparentemail");
                var body = new
                {
                    parentEmail,
                    verificationCode
                };
                var res = await SendPostRequest<AssignParentEmailResponse>(url, body);
                if (res.IsError) return AssignParentEmailResult.Error(res.ErrorMessage);
                return AssignParentEmailResult.Success(res.Model.NewEmailCode);
            }
            catch (Exception e)
            {
                return AssignParentEmailResult.Error(e.Message);
            }
        }

        public async Task<Result<ValidatePasswordStrengthResult>> ValidatePasswordStrength(string password, string nickname)
        {
            try
            {
                var url = UrlCombine(AuthUrl, "account/ValidatePassword");
                var body = new
                {
                    password,
                    UserName = nickname
                };
                var res = await SendPostRequest<ValidatePasswordStrengthResult>(url, body);
                if (res.IsError) return Result<ValidatePasswordStrengthResult>.Error(res.ErrorMessage, res.HttpStatusCode);
                return Result<ValidatePasswordStrengthResult>.Success(res.Model);
            }
            catch (Exception e)
            {
                return Result<ValidatePasswordStrengthResult>.Error(e.Message);
            } 
        }
        
        [UsedImplicitly]
        private sealed class AssignParentEmailResponse
        {
            public string NewEmailCode { get; set; }
        }

        public async Task<Result<bool>> CheckIfParentEmailBound(string username)
        {
            var url = UrlCombine(AuthUrl, "account/CheckIfParentEmailBound");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            var body = new
            {
                username,
            };
            req.AddJsonContent(Serializer.SerializeToJson(body));
            AddDeviceIdHeader(req);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<bool>.Error(resp.Message);
            }

            var responseModel = Serializer.DeserializeJson<ParentEmailCheckResponse>(resp.DataAsText);
            return Result<bool>.Success(responseModel.isLoginByParentEmailAvailable);
        }

        public async Task<Result> ConfigureParentalConsent(ConfigureParentalConsentRequest configureParentalConsentRequest)
        {
            try
            {
                var url = Extensions.CombineUrls(AuthUrl, "account/ConfigureParentalConsent");
                return await SendPostRequest(url, configureParentalConsentRequest);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> RegisterTemporaryAccount(TemporaryAccountRequestModel model)
        {
            var url = UrlCombine(AuthUrl, "account/RegisterTemporaryAccount");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, false, false);
            req.AddJsonContent(Serializer.SerializeToJson(model));
            AddDeviceIdHeader(req);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.Message, resp.StatusCode);
            }

            var tokenResponse = Serializer.DeserializeJson<TokenResponse>(resp.DataAsText);
            SetData(tokenResponse);
            return new SuccessResult();
        }

        public async Task<UpdateUsernameResult> UpdateUsername(string username)
        {
            var url = UrlCombine(AuthUrl, "/api/credential/username");
            var body = new
            {
                Username = username,
            };

            var response = await SendPostRequest<UpdateUsernameResult>(url, body);
            
            return response.IsError ? DeserializeJson<UpdateUsernameResult>(response.ErrorMessage) : response.Model;
        }

        public async Task<Result> UpdateUserInfo(UpdateUserModel model)
        {
            var url = UrlCombine(AuthUrl, "account/update");
            return await SendPutRequest(url, model);
        }

        private async Task<LoginResult> LoginAsync()
        {
            var renewTokenResult = await RenewToken();

            if (renewTokenResult.IsError)
                return new LoginResult(renewTokenResult.ErrorMessage);

            var data = renewTokenResult.ResultObject;
            SetData(data);

            return new LoginResult();
        }

        private void SetData(TokenResponse tokenResp)
        {
            MainServerHost = BuildUrl(tokenResp.server_url);
            AssetManagerServerHost = BuildUrl(tokenResp.assetmanager_server);
            ClientServerHost = BuildUrl(tokenResp.client_server);
            AssetSeverHost = BuildUrl(tokenResp.asset_server);
            UserServerHost = BuildUrl(tokenResp.social_server);
            NotificationServerHost = BuildUrl(tokenResp.notification_server);
            VideoServerHost = tokenResp.video_server;
            if (tokenResp.chat_server != null)//temp check, not needed when we have uploaded chat feature to prod backend
            {
                ChatServerHost = BuildUrl(tokenResp.chat_server);
            }
            Token = new AuthToken(tokenResp.access_token, tokenResp.refresh_token);

            var userInfo = ParseToken(tokenResp.access_token);
            Profile.Id = userInfo.UserId;
            Profile.Email = userInfo.Email;
            Profile.GroupId = userInfo.PrimaryGroupId;
            Profile.IsQA = userInfo.QA;
            Profile.IsArtist = userInfo.Artist;
            Profile.IsModerator = userInfo.Moderator;
            Profile.IsEmployee = userInfo.Employee;
            Profile.RegisteredWithAppleId = userInfo.RegisteredWithAppleId;
            Profile.MainCharacterId = userInfo.MainCharacterId;
            Profile.IsOnboardingCompleted = userInfo.OnboardingCompleted;
        }

        private string BuildUrl(string host)
        {
            return UrlCombine(host, "api/");
        }

        private string UrlCombine(string url1, string url2)
        {
            return Extensions.CombineUrls(url1, url2);
        }

        private async Task<GenericResult<TokenResponse>> RenewToken()
        {
            if (Token.RefreshToken == null) return new GenericResult<TokenResponse>("Refresh token is null");

            using (var client = new HttpClient())
            {
                var url = UrlCombine(AuthUrl, "connect/token");
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var byteArray = new UTF8Encoding().GetBytes(ClientIdAndSecret);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var formData = new List<KeyValuePair<string, string>>
                {
                    new("refresh_token", Token.RefreshToken),
                    new("grant_type", "refresh_token"),
                    new("scope", "friends_factory.creators_api offline_access")
                };

                request.Content = new FormUrlEncodedContent(formData);
                AddDeviceIdHeader(client);
                var resp = await client.SendAsync(request);

                if (!resp.IsSuccessStatusCode) throw new InvalidOperationException(resp.ReasonPhrase);

                var json = await resp.Content.ReadAsStringAsync();

                var respModel = JsonUtility.FromJson<TokenResponse>(json);
                return new GenericResult<TokenResponse>(respModel);
            }
        }

        private UserInfo ParseToken(string token)
        {
            var parts = token.Split('.');
            if (parts.Length > 2)
            {
                var decode = parts[1];
                var padLength = 4 - decode.Length % 4;
                if (padLength < 4) decode += new string('=', padLength);

                var bytes = Convert.FromBase64String(decode);
                var userInfoText = Encoding.ASCII.GetString(bytes);

                return Serializer.DeserializeJson<UserInfo>(userInfoText);
            }

            Debug.LogError("Can't parse token. Something wrong from auth server");

            return new UserInfo();
        }

        private static void AddDeviceIdHeader(HTTPRequest request)
        {
            request.AddHeader(DEVICE_ID_HEADER, DeviceId);
        }
        
        private static void AddDeviceIdHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Add(DEVICE_ID_HEADER, DeviceId);
        }
    }

    internal struct VerifyEmailRequest
    {
        public string Email { get; set; }
    }

    internal struct ValidationInvitationCodeResp
    {
        public bool IsValid { get; set; }

        public string Error { get; set; }
    }
    
}
