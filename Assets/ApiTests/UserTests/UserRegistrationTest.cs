using System;
using System.Threading.Tasks;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.UserTests
{
    public class UserRegistrationTest : AnonymousAccessedEndPointTest
    {
        public string Email;
        public string NickName;
        
        protected override async void RunTestAsync()
        {
            var reqModel = new UserRegistrationModel();
            reqModel.Credentials = await GetCredentials();
            reqModel.BirthDate = DateTime.Now;
            reqModel.UserName = NickName;

            var resp = await Bridge.RegisterAsync(
                reqModel, true);

            Debug.Log(resp.ErrorType);
        }

        protected override Task<ICredentials> GetCredentials()
        {
            var creds = new EmailCredentials
            {
                Email = Email
            };

            return Task.FromResult((ICredentials) creds);
        }
    }
}