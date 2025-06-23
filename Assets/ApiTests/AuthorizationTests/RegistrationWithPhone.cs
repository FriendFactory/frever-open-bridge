using System;
using System.Threading.Tasks;
using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class RegistrationWithPhone: MonoBehaviour
    {
        public FFEnvironment Environment;
        public string UserName;
        public string PhoneNumber;
        public string VerificationCode;
        
        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(Environment);

            await bridge.RequestPhoneNumberVerificationCode(PhoneNumber);

            while (string.IsNullOrEmpty(VerificationCode) || VerificationCode.Length!=6)
            {
                await Task.Delay(100);
            }
            
            var creds = new PhoneNumberCredentials(PhoneNumber, VerificationCode);
            var userRegModel = new UserRegistrationModel();
            userRegModel.Credentials = creds;
            userRegModel.BirthDate = DateTime.Now - TimeSpan.FromDays(10000);
            userRegModel.UserName = UserName;
            await bridge.RegisterAsync(userRegModel, false);
        }
    }
}