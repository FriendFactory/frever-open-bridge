using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    internal sealed class SendEmailAndAppleIdTest : AnonymousAccessedEndPointTest
    {
        public string Email = "serhii.test@email.com";
        public string AppleId = "000101.020100.1020.2";
    
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.StoreEmailForAppleId(AppleId, Email);
            if (resp.IsSuccess)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError($"Error: {resp.ErrorMessage}");
            }
        }
    }
}
