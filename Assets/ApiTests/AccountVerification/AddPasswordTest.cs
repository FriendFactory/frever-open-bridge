using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.AccountVerification
{
    public class AddPasswordTest: VerificationMethodTestBase
    {
        [SerializeField] private string _password;
        
        protected override async void RunTestAsync()
        {
            try
            {
                var result = await Bridge.AddVerificationMethod(_password);
                if (result.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to verify password # code: {result.HttpStatusCode}, msg: {result.ErrorMessage}");
                    return;
                }
    
                var credentialStatus = await GetCredentialStatusAsync();
                
                if (credentialStatus == null) return;
                
                Assert.IsTrue(credentialStatus.HasPassword);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}