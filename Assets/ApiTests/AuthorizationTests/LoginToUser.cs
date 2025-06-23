using System.Threading.Tasks;
using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public sealed class LoginToUser : MonoBehaviour
    {
        public string Email;
        public FFEnvironment Environment = FFEnvironment.Develop;
        public string VerificationCode;

        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(Environment);

            await bridge.RequestEmailVerificationCode(Email);
            while (VerificationCode == null || VerificationCode.Length != 6)
            {
                await Task.Delay(33);
            }
            
            var resp = await bridge.LogInAsync(new EmailCredentials
            {
                Email = Email,
                VerificationCode = VerificationCode
            }, true);

            if (resp.IsError)
            {
                Debug.LogError("Error: " + resp.ErrorType);
            }
            else
            {
                Debug.Log("Logged in");
            }
        }
    }
}