using System;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.UserTests
{
    public sealed class RegistrationWithEmailAndVerificationCode: AnonymousAccessedEndPointTest
    {
        public string Email;
        public string VerificatyionCode;
        public string InvitationCode;

        protected override async void RunTestAsync()
        {
            var requestVerification = await Bridge.RequestEmailVerificationCode(Email);
            if (requestVerification.IsError) throw new InvalidOperationException(requestVerification.ErrorMessage);
            
            while (string.IsNullOrEmpty(VerificatyionCode) || VerificatyionCode.Length!=6)
            {
                await Task.Delay(30);
            }
            
            var registrationRequest = new UserRegistrationModel()
            {
                AllowDataCollection = false,
                AnalyticsEnabled = false,
                BirthDate = DateTime.Now - TimeSpan.FromDays(1000),
                UserName = "Serhii bridge",
                Credentials = new EmailCredentials()
                {
                    VerificationCode = VerificatyionCode,
                    Email = Email
                }
            };

            var resp = await Bridge.RegisterAsync(registrationRequest, false);
            Debug.Log($"IsSuccess: {resp.IsSuccess}");
        }
    }
}