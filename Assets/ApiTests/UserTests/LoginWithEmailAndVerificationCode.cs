using System;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.UserTests
{
    public sealed class LoginWithEmailAndVerificationCode: ApiTestBase
    {
        public string Email;
        public string VerificatyionCode;
        
        protected override void Start()
        {
            RunTestAsync();
        }

        protected override async void RunTestAsync()
        {
            var requestVerification = await Bridge.RequestEmailVerificationCode(Email);
            if (requestVerification.IsError) throw new InvalidOperationException(requestVerification.ErrorMessage);
            
            while (string.IsNullOrEmpty(VerificatyionCode) || VerificatyionCode.Length!=6)
            {
                await Task.Delay(30);
            }

            var creds = new EmailCredentials()
            {
                VerificationCode = VerificatyionCode,
                Email = Email
            };
            var resp = await Bridge.LogInAsync(creds, false);
            Debug.Log($"IsSuccess: {resp.IsSuccess}");
        }

        protected override Task<ICredentials> GetCredentials()
        {
            throw new NotImplementedException();
        }
    }
}