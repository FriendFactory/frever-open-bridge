using System.Threading.Tasks;
using Bridge.AccountVerification.Models;
using UnityEngine;

namespace ApiTests.AccountVerification
{
    public abstract class VerificationMethodTestBase: AuthorizedUserApiTestBase
    {
        protected abstract override void RunTestAsync();

        protected async Task<CredentialStatus> GetCredentialStatusAsync()
        {
            var credentialsStatusResult = await Bridge.GetCredentialsStatus();
            if (credentialsStatusResult.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get credentials status # code: {credentialsStatusResult.HttpStatusCode}, msg: {credentialsStatusResult.ErrorMessage}");
                return null;
            }

            return credentialsStatusResult.Model;
        }
    }
}