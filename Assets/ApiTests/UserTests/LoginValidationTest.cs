using System.Threading.Tasks;
using Bridge.Authorization.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.UserTests
{
    public sealed class LoginValidationTest: ApiTestBase
    {
        public string PhoneNumber;
        public string Email;
        public string UserName;
        protected override void Start()
        {
            RunTestAsync();
        }

        protected override async void RunTestAsync()
        {
            var validationModel = new ValidationModel
            {
                Email = Email, 
                UserName = UserName, 
                PhoneNumber = PhoneNumber
            };
            var result = await Bridge.ValidateLoginCredentials(validationModel);
            Debug.Log(result.IsError ? result.ErrorMessage : JsonConvert.SerializeObject(result));
        }

        protected override Task<ICredentials> GetCredentials()
        {
            throw new System.NotImplementedException();
        }
    }
}