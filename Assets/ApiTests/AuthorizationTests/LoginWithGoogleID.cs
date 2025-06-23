using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class LoginWithGoogleID : MonoBehaviour
    {
        public FFEnvironment Environment = FFEnvironment.Develop;
        public string GoogleID;
        public string IdentityToken;
        public string UserName;
        public string InvitationCode;
        
        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(Environment);

            var resp = await bridge.LogInAsync(new GoogleAuthCredentials(UserName, IdentityToken, GoogleID), true);
            
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