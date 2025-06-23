using System;
using Bridge.Authorization.Models;

namespace ApiTests.AuthorizationTests
{
    public class RegistrationWithApple : AnonymousAccessedEndPointTest
    {
        public string UserName;
        public string AppleID;
        public string IdentityToken;
       
        protected override async void RunTestAsync()
        {
            var creds = new AppleAuthCredentials(UserName, IdentityToken, AppleID);
            var userRegModel = new UserRegistrationModel();
            userRegModel.Credentials = creds;
            userRegModel.BirthDate = DateTime.Now - TimeSpan.FromDays(10000);
            userRegModel.UserName = UserName;
            await Bridge.RegisterAsync(userRegModel, false);
        }
    }
}