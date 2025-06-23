using System;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests
{
    public abstract class AuthorizedUserApiTestBase : ApiTestBase
    {
        [DrawIf(nameof(UseLastSavedUser), false)]
        public string Email;
        
        [DrawIf(nameof(UseLastSavedUser), false)]
        public string VerificationCode;
        
        protected override async Task<ICredentials> GetCredentials()
        {
            PlayerPrefs.SetString("EMAIL", Email);
            PlayerPrefs.Save();
            await Bridge.RequestEmailVerificationCode(Email);
            while (VerificationCode == null || VerificationCode.Length!=6)
            {
                //wait when verification code it put
                await Task.Delay(30);
            }

            var credentials = new EmailCredentials()
            {
                Email = Email,
                VerificationCode = VerificationCode
            };
            return credentials;
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Email) && PlayerPrefs.HasKey("EMAIL"))
            {
                Email = PlayerPrefs.GetString("EMAIL");
            }
        }
    }
}