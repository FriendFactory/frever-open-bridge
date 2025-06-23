using System;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.AccountVerification
{
    public sealed class GetCredentialsStatusTest : VerificationMethodTestBase 
    {
        protected override async void RunTestAsync()
        {
            try
            {
                var credentialStatus = await GetCredentialStatusAsync();
            
                Debug.Log($"[{GetType().Name}] Credential status:\n {JsonConvert.SerializeObject(credentialStatus)}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
