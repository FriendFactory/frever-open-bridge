using System.IO;
using System.Threading.Tasks;
using Bridge;
using Bridge.Authorization.Models;
using Bridge.Authorization.Results;
using UnityEngine;

namespace ApiTests
{
    public abstract class ApiTestBase: MonoBehaviour
    {
        public FFEnvironment Environment = FFEnvironment.Develop;
        public bool UseLastSavedUser = true;
        public bool RememberUser = true;
        
        protected IBridge Bridge;

        private void Awake()
        {
            Bridge = new ServerBridge();
            // if (Environment == FFEnvironment.ContentProduction || Environment == FFEnvironment.Production)
            //     throw new Exception("Avoid to run ");
            Bridge.ChangeEnvironment(Environment);
        }

        protected virtual async void Start()
        {
            var loginResult = await Login();

            if (loginResult.IsError)
            {
                Debug.LogError($"Can't login. Error message: {loginResult.ErrorMessage}");
                return;
            }

            RunTestAsync();
        }

        private async Task<LoginResult> Login()
        {
            LoginResult loginResult;
            if (UseLastSavedUser)
            {
                loginResult = await LoginToLastSaved();
                if (loginResult.IsError)
                {
                    loginResult = await LoginWithCredentials();
                }
            }
            else
            {
                loginResult = await LoginWithCredentials();
            }

            return loginResult;
        }

        private async Task<LoginResult> LoginWithCredentials()
        {
            var creds = await GetCredentials();
            return await Bridge.LogInAsync(creds, RememberUser);
        }

        private Task<LoginResult> LoginToLastSaved()
        {
            return Bridge.LoginToLastSavedUserAsync();
        }

        protected abstract void RunTestAsync();

        protected abstract Task<ICredentials> GetCredentials();
        
        protected string GetFilePath(string fileName)
        {
            return Path.Combine(Application.streamingAssetsPath, "TestFiles", fileName);
        }
    }
}