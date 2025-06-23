using System;
using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class LoginWithAppleID : MonoBehaviour
    {
        public FFEnvironment Environment = FFEnvironment.Develop;
        public string AppleID;
        public string IdentityToken;
        public string UserName;
        public string InvitationCode;
        
        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(Environment);

            var resp = await bridge.LogInAsync(new AppleAuthCredentials(UserName, IdentityToken,  AppleID), true);
            
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