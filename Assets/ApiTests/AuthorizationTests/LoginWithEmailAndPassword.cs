using System.Threading.Tasks;
using Bridge.Authorization.Models;

namespace ApiTests.AuthorizationTests
{
    public class LoginWithEmailAndPassword: ApiTestBase
    {
        public string Email;
        public string Password;

        protected override async void Start()
        {
            var creds = new EmailAndPasswordCredentials()
            {
                Email = Email,
                Password = Password
            };

            await Bridge.LogInAsync(creds, false);
        }

        protected override void RunTestAsync()
        {
            
        }

        protected override Task<ICredentials> GetCredentials()
        {
            throw new System.NotImplementedException();
        }
    }
}