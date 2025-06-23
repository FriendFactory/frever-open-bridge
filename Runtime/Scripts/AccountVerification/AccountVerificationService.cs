using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.AccountVerification.Models;
using Bridge.Authorization;
using Bridge.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.AccountVerification
{
    internal sealed class AccountVerificationService: ServiceBase, IAccountVerificationService
    {
        private readonly string _baseUrl;
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;
        
        public AccountVerificationService(string authUrl, IRequestHelper requestHelper, ISerializer serializer) : base(requestHelper, serializer)
        {
            _baseUrl = ConcatUrl(authUrl, "/api/credential");
        }

        public async Task<Result<CredentialStatus>> GetCredentialsStatus(CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(_baseUrl, $"status");
                
                return await SendRequestForSingleModel<CredentialStatus>(url, token, useProtobuf: false);
            }
            catch (Exception e)
            {
                return Result<CredentialStatus>.Error(e.Message);
            }
        }

        public async Task<Result> AddCredentials(AddCredentialsRequest request)
        {
            try
            {
                var url = _baseUrl;
                
                return await SendPostRequest(url, request);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> SendVerificationCode(VerifyCredentialRequest request)
        {
            try
            {
                var url = ConcatUrl(_baseUrl, "verify");
                
                return await SendPostRequest(url, request);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> UpdateUserCredentials(UpdateCredentialsRequest request)
        {
            try
            {
                var url = _baseUrl;
                
                return await SendPutRequest(url, request);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result<VerifyUserResponse>> VerifyUserCredentials(VerifyUserRequest request)
        {
            try
            {
                var url = ConcatUrl(_baseUrl, "verify/user");
                
                return await SendPostRequest<VerifyUserResponse>(url, request);
            }
            catch (Exception e)
            {
                return Result<VerifyUserResponse>.Error(e.Message);
            }
        }
    }
}