using Bridge.Authorization.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.UserTests
{
    public sealed class RegistrationValidationTest: AnonymousAccessedEndPointTest
    {
        public string PhoneNumber;
        public string Email;
        public string UserName;

        protected override async void RunTestAsync()
        {
            var validationModel = new ValidationModel
            {
                Email = Email, 
                UserName = UserName, 
                PhoneNumber = PhoneNumber
            };
            var result = await Bridge.ValidateRegistrationCredentials(validationModel);
            Debug.Log(result.IsError ? result.ErrorMessage : JsonConvert.SerializeObject(result));
        }
    }
}